using BookHubAPI.Models;
using BookHubAPI.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ReviewHubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewsController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviewById(string id)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        [HttpPost]
        [Route("addReview")]
        public async Task<ActionResult<Review>> AddReview(Review review)
        {
            var addedReview = await _reviewRepository.AddReview(review);
            return CreatedAtAction(nameof(GetReviewById), new { id = addedReview.RevId }, addedReview);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Review>> UpdateReview(string id, Review review)
        {
            if (id != review.RevId)
            {
                return BadRequest();
            }
            var updatedReview = await _reviewRepository.UpdateReview(id, review);
            if (updatedReview == null)
            {
                return NotFound();
            }
            return Ok(updatedReview);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Review>> PatchReview(string id, JsonPatchDocument<Review> patchDocument)
        {
            var review = await _reviewRepository.GetReviewById(id);
            if (review == null)
            {
                return NotFound();
            }

            patchDocument.ApplyTo(review, ModelState);

            if (!TryValidateModel(review))
            {
                return BadRequest(ModelState);
            }

            var updatedReview = await _reviewRepository.UpdateReview(id, review);
            return Ok(updatedReview);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReview(string id)
        {
            var result = await _reviewRepository.DeleteReview(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
