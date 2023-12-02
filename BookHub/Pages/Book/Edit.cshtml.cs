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
            using (var client = new HttpClient())
            {
                var reviewJson = JsonConvert.SerializeObject(Review);
                var content = new StringContent(reviewJson, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"https://localhost:7274/api/Reviews/addReview", content);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle error scenario
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
