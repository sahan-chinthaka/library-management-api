using library_management_api.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace library_management_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly LibraryDBContext dbContext;

        public DashboardController(LibraryDBContext context) => dbContext = context;

        [HttpGet]
        public IActionResult GetBookCount()
        {
            var bookCount = dbContext.Books.Count();
            return Ok(bookCount);
        }
    }
}
