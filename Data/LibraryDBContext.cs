using library_management_api.Models;
using Microsoft.EntityFrameworkCore;

namespace library_management_api.Data
{
    public class LibraryDBContext : DbContext
    {
        public LibraryDBContext(DbContextOptions<LibraryDBContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
    }
}
