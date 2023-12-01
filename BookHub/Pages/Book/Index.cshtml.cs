using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookHub.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json; // Update with the correct namespace for BookModel

namespace BookHub.Pages.Book
{
    public class IndexModel : PageModel
    {
        public List<BookModel> Books { get; set; } = new List<BookModel>(); // Assuming Book is your model class

        public async Task<IActionResult> OnGetAsync()
        {
            // Retrieve all books from the API
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:7274/api/Book/getitems");
                if (response.IsSuccessStatusCode)
                {
                    var booksJson = await response.Content.ReadAsStringAsync();
                    Books = JsonConvert.DeserializeObject<List<BookModel>>(booksJson);
                }
            }
            return Page();
        }
    }
}
