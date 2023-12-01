using BookHub.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace BookHub.Pages.Book
{
    public class DetailsModel : PageModel
    {
        public BookModel Book { get; set; } // Assuming Book is your model class

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
            return Page();
        }
    }
}
