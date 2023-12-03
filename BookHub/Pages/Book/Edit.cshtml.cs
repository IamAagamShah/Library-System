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

        public string id { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            this.id = id;
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
                if (Review == null)
                {
                    Review = new ReviewModel(); // Or initialize it accordingly
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            Review.Id = id;
            Book.Id = id;
            // Update book by ID using the API
            using (var client = new HttpClient())
            {
                var bookJson = JsonConvert.SerializeObject(Book);
                var content = new StringContent(bookJson, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"https://localhost:7274/api/Book/{id}", content);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle error scenario for book update
                }
            }


            using (var client = new HttpClient())
            {
                // Check if a review exists with the provided Review.Id
                var reviewResponse = await client.GetAsync($"https://localhost:7274/api/Reviews/{Review.Id}");
                if (reviewResponse.IsSuccessStatusCode)
                {
                    var existingReviewJson = await reviewResponse.Content.ReadAsStringAsync();
                    var existingReview = JsonConvert.DeserializeObject<ReviewModel>(existingReviewJson);

                    // Update the description if the review exists
                    if (existingReview != null)
                    {
                        existingReview.Description = Review.Description;

                        var existingReviewJsonUpdated = JsonConvert.SerializeObject(existingReview);
                        var existingReviewContent = new StringContent(existingReviewJsonUpdated, Encoding.UTF8, "application/json");

                        var updateReviewResponse = await client.PutAsync($"https://localhost:7274/api/Reviews/{Review.Id}", existingReviewContent);
                        if (!updateReviewResponse.IsSuccessStatusCode)
                        {
                            // Handle error scenario for update
                        }
                    }
                }
                else
                {
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
                return RedirectToPage("./Index");
            }

        }
    }

