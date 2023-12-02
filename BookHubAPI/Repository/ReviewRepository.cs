using AutoMapper;
using BookHubAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace BookHubAPI.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ReviewsDbContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(ReviewsDbContext reviewsDbContext, IMapper mapper)
        {
            _context = reviewsDbContext;
            _mapper = mapper;
        }

        public async Task<Review> PartialUpdateReviewAsync(string id, Review review)
        {
            var existingReview = await _context.Reviews.FirstOrDefaultAsync(b => b.Id == id);
            if (existingReview == null) return null;

            // Update other properties similarly...

            await _context.SaveChangesAsync();
            return existingReview;
        }

        public async Task<bool> DeleteReview(string id)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(b => b.Id == id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Review> GetReviewById(string id, bool includeRevies)
        {
            IQueryable<Review> result;

            if (includeRevies)
            {
                result = _context.Reviews.Include(c => c.Id).Where(c => c.Id == id);
            }
            else
            {
                result = _context.Reviews.Where(c => c.Id == id);
            }
            return await result.FirstOrDefaultAsync();
        }

        public async Task<Review> AddReview(Review review)
        {
            _context.Database.OpenConnection();
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Reviews ON");

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Reviews OFF");
            _context.Database.CloseConnection();

            return review;
        }

        public async Task<Review> UpdateReview(string id, Review review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return review;

        }

    }
}
