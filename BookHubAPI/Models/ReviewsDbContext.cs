using Microsoft.EntityFrameworkCore;

namespace BookHubAPI.Models
{
    public class ReviewsDbContext : DbContext
    {
        public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : base(options)
        {
            // Other configuration or setup if needed
        }
        public DbSet<Review> Reviews { get; set; } // DbSet for the Book entity

    }
}
