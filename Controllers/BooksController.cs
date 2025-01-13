using library_management_api.Data;
using library_management_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("{id}")]
        public IActionResult GetBook(int id)
        {
            return Ok(dbContext.Books.Find(id));
        }

        [HttpPost]
        public IActionResult AddBook(Book book)
        {
            dbContext.Books.Add(book);
            dbContext.SaveChanges();
            return Created();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, Book book)
        {
            var existBook = dbContext.Books.Find(id);
            if (existBook == null) return NotFound($"No book with id: {id}");

            existBook.Author = book.Author;
            existBook.Publisher = book.Publisher;
            existBook.CreatedDate = book.CreatedDate;
            existBook.Description = book.Description;
            existBook.Name = book.Name;

            dbContext.SaveChanges();

            return Ok($"Update book with id: {id}");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var book = dbContext.Books.Find(id);
            if (book == null) return NotFound($"No book with id: {id}");

            dbContext.Books.Remove(book);
            return Ok($"Delete book with id: {id}");
        }
    }
}
