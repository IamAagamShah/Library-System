using BookHubAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BookHubAPI.Repository
{ 
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewsDbContext _context;
        public ReviewRepository(ReviewsDbContext reviewsDbContext) {
            _context = reviewsDbContext;
        }

        public async Task<Review> AddReview(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteReview(string id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(b => b.RevId == id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Review> UpdateReview(string id, JsonPatchDocument<Review> patchDocument)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(b => b.RevId == id);
            if (review == null) return null;

            patchDocument.ApplyTo(review);

            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<Review> GetReviewById(string id)
        {
            return await _context.Reviews.FirstOrDefaultAsync(b => b.RevId == id);
        }

        public async Task<Review> UpdateReview(string id, Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return review;
        }
    }
}
