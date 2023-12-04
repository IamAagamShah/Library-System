using BookHub.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;

namespace BookHub.Pages.Book
{
    public class CreateModel : PageModel
    {
        private readonly HttpClient _client;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public BookModel Book { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {

            Book.Link = "Link is not specified";
            Book.Image = "It is not specified";
            Book.Rating = 1;

            try
            {
                var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(Book), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync("https://localhost:7274/api/Book/addbook", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("./Index");
                }
                else
                {
                    // Handle error scenario
                    // For example, set ModelState errors
                    ModelState.AddModelError(string.Empty, "Error adding book. Please try again.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                ModelState.AddModelError(string.Empty, "An error occurred. Please try again later.");
                return Page();
            }
        }
    }
}