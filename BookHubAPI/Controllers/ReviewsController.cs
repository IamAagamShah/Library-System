using AutoMapper;
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
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
       
        public ReviewsController(IReviewRepository reviewRepository, IMapper mapper, IBookRepository bookRepository)
        {
            _reviewRepository = reviewRepository ?? throw new ArgumentNullException(nameof(reviewRepository));
            _mapper = mapper;
            _bookRepository = bookRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReviewById(string id, bool includeReviews = false)
        {
            var review = await _reviewRepository.GetReviewById(id, includeReviews);
            if (review == null)
            {
                return NotFound();
            }
            if (includeReviews)
            {
                var reviews = _mapper.Map<ReviewDTO>(review);
                return Ok(reviews);
            }
            return Ok(review);

        }

        [HttpPost]
        [Route("addReview")]
        public async Task<ActionResult<Review>> AddReview(ReviewDTO reviewDto)
        {
            var review = _mapper.Map<Review>(reviewDto);
            var addedReview = await _reviewRepository.AddReview(review);
            return CreatedAtAction(nameof(GetReviewById), new { id = addedReview.Id }, addedReview);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Review>> UpdateReview(string id, UpdateReviewDTO reviewDto)
        {
            //if (id != reviewDto.Id)
            //{
            //    return BadRequest();
            //}

            var existingReview = await _reviewRepository.GetReviewById(id, false);
            if (existingReview == null)
            {
                return NotFound();
            }

            existingReview.Description = reviewDto.Description;
            //_mapper.Map(reviewDto, existingReview);
            //var updatedReview = await _reviewRepository.UpdateReview(id, reviewDto);
            //if (updatedReview == null)
            //{
            //    return NotFound();
            //}
            // Ensure Id (identity column) is not included in update process
           
            var updatedReview = await _reviewRepository.UpdateReview(id, existingReview);
            var updatedReviewDto = _mapper.Map<ReviewDTO>(updatedReview);
            return Ok(updatedReviewDto);

        }

        //[HttpPatch("{id}")]
        //public async Task<ActionResult<Review>> PatchReview(string id, JsonPatchDocument<Review> patchDocument)
        //{
        //    var review = await _reviewRepository.GetReviewById(id);
        //    if (review == null)
        //    {
        //        return NotFound();
        //    }

        //    var reviewDto = _mapper.Map<ReviewDTO>(review);
        //    patchDocument.ApplyTo(reviewDto);

        //    if (!TryValidateModel(reviewDto))
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var updatedReview = _mapper.Map<ReviewDTO>(reviewDto);
        //    var updatedReviewFromRepo = await _reviewRepository.UpdateReview(id, updatedReview);

        //    var updatedReviewDto = _mapper.Map<ReviewDTO>(updatedReviewFromRepo);
        //    return Ok(updatedReviewDto);
        //}

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
