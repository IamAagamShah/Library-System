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
            return RedirectToPage("./Index");
        }
    }
}
