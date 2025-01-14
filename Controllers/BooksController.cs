using library_management_api.Data;
using library_management_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace library_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDBContext dbContext;

        public BooksController(LibraryDBContext context) => dbContext = context;

        [HttpGet]
        public IActionResult GetBooks(string? name = null)
        {
            var query = dbContext.Books.AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(b => b.Name.Contains(name));
            }
            return Ok(query.ToList());
        }

        [HttpGet("mybooks")]
        [Authorize]
        public async Task<IActionResult> GetUserBooks()
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var userBooks = await dbContext.Books
                .Where(b => b.UserId == int.Parse(userId))
                .ToListAsync();

            return Ok(userBooks);
        }


        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            return Ok(dbContext.Books.Find(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBook(BookDto bookDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }
            var user = await dbContext.Users.FindAsync(int.Parse(userId));
            if (user == null)
            {
                return Unauthorized("User not found");
            }
            var book = new Book
            {
                Name = bookDto.Name,
                Author = bookDto.Author,
                Description = bookDto.Description,
                ImageURl = bookDto.ImageURl,
                CreatedDate = bookDto.CreatedDate,
                Publisher = bookDto.Publisher,
                UserId = int.Parse(userId),
                User = user
            };

            dbContext.Books.Add(book);
            dbContext.SaveChanges();
            return Created();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id, Book book)
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var existBook = await dbContext.Books.FindAsync(id);
            if (existBook == null) return NotFound($"No book with id: {id}");

            if (existBook.UserId != int.Parse(userId))
            {
                return Forbid("You are not authorized to update this book.");
            }

            existBook.Author = book.Author;
            existBook.Publisher = book.Publisher;
            existBook.CreatedDate = book.CreatedDate;
            existBook.Description = book.Description;
            existBook.Name = book.Name;

            await dbContext.SaveChangesAsync();

            return Ok($"Update book with id: {id}");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }

            var book = await dbContext.Books.FindAsync(id);
            if (book == null) return NotFound($"No book with id: {id}");

            if (book.UserId != int.Parse(userId))
            {
                return Forbid("You are not authorized to delete this book.");
            }

            dbContext.Books.Remove(book);
            await dbContext.SaveChangesAsync();

            return Ok($"Delete book with id: {id}");
        }

    }
}
