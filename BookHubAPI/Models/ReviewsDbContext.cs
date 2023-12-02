using Microsoft.EntityFrameworkCore;

namespace BookHubAPI.Models
{
    public class ReviewsDbContext : DbContext
    {
        public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : base(options)
        {
            
        }
        public DbSet<Review> Reviews { get; set; } // DbSet for the Book entity

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Review>()
                .Property(r => r.RevId)
                .ValueGeneratedOnAdd();

            // Other configurations for your entities can go here...

            base.OnModelCreating(modelBuilder);
        }

    }
}
