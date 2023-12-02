using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BookHub.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BookHub.Pages.Book
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public BookModel Book { get; set; } // Assuming Book is your model class
        [BindProperty]
        public ReviewModel Review { get; set; } // Assuming Review is your model class

        public async Task<IActionResult> OnGetAsync(string id)
        {
            // Retrieve book by ID from the API
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"https://localhost:7274/api/Book/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var bookJson = await response.Content.ReadAsStringAsync();
                    Book = JsonConvert.DeserializeObject<BookModel>(bookJson);
                }
            }

            // Retrieve review data based on the book ID from the API
            using (var client = new HttpClient())
            {
                var reviewResponse = await client.GetAsync($"https://localhost:7274/api/Reviews/{id}?includeReviews=false");
                if (reviewResponse.IsSuccessStatusCode)
                {
                    var reviewJson = await reviewResponse.Content.ReadAsStringAsync();
                    Review = JsonConvert.DeserializeObject<ReviewModel>(reviewJson);
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Update book by ID using the API
            using (var client = new HttpClient())
            {
                var bookJson = JsonConvert.SerializeObject(Book);
                var content = new StringContent(bookJson, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://localhost:7274/api/Book/{Book.Id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle error scenario
                }
            }

            // Update review data based on the book ID using the API
            // Check if a review is available for the bookId
            using (var client = new HttpClient())
            {
                var reviewResponse = await client.GetAsync($"https://localhost:7274/api/Reviews/{Review.Id}?includeReviews=false");
                if (reviewResponse.IsSuccessStatusCode)
                {
                    var reviewJson = await reviewResponse.Content.ReadAsStringAsync();
                    ReviewModel existingReview = JsonConvert.DeserializeObject<ReviewModel>(reviewJson);

                    // Update the Description property of the Review model
                    if (existingReview != null)
                    {
                        existingReview.Description = Review.Description; // Assigning the new description

                        // Review exists, update the existing review
                        var existingReviewJson = JsonConvert.SerializeObject(existingReview);
                        var existingReviewContent = new StringContent(existingReviewJson, Encoding.UTF8, "application/json");

                        var updateReviewResponse = await client.PutAsync($"https://localhost:7274/api/Reviews/{Review.Id}", existingReviewContent);
                        if (!updateReviewResponse.IsSuccessStatusCode)
                        {
                            // Handle error scenario for update
                        }
                }
                else
                {
                    existingReview.Description = Review.Description;
                    // Review doesn't exist, add a new review
                    var newReviewJson = JsonConvert.SerializeObject(Review);
                    var newReviewContent = new StringContent(newReviewJson, Encoding.UTF8, "application/json");

                    var addReviewResponse = await client.PostAsync("https://localhost:7274/api/Reviews/addReview", newReviewContent);
                    if (!addReviewResponse.IsSuccessStatusCode)
                    {
                          // Handle error scenario for addition
                    }
                }
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
