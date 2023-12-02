using BookHubAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookHubAPI.Repository
{
    public interface IReviewRepository
    {
        Task<Review> GetReviewById(string id, bool includeRevies);
        Task<Review> AddReview(Review review);
        Task<Review> UpdateReview(string id, Review review);
        Task<bool> DeleteReview(string id);
    }
}
