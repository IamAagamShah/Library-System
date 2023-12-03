using BookHubAPI.Models;
using Microsoft.EntityFrameworkCore;

public class ReviewsDbContext : DbContext
{
    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options) : base(options)
    {
    }
    public DbSet<Review> Reviews { get; set; }
}
