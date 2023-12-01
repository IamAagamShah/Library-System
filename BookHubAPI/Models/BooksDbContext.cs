using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace BookHubAPI.Models
{
    public class BooksDbContext : DbContext
    {
        public BooksDbContext(DbContextOptions<BooksDbContext> options) : base(options)
        {
            // Other configuration or setup if needed
        }
        public DbSet<Book> Books { get; set; } // DbSet for the Book entity

    }
}
