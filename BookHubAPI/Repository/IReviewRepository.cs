using BookHubAPI.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace BookHubAPI.Repository
{
    public interface IReviewRepository
    {
        Task<Review> GetReviewById(string id);
        Task<Review> AddReview(Review review);
        Task<Review> UpdateReview(string id, JsonPatchDocument<Review> patchDocument);
        Task<Review> UpdateReview(string id, Review Review);
        Task<bool> DeleteReview(string id);
    }
}
