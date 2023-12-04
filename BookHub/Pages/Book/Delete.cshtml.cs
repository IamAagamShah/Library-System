using BookHub.Pages.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;

namespace BookHub.Pages.Book
{
    public class DeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DeleteModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public BookModel Book { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.GetAsync($"https://localhost:32768/api/Book/{id}");

            if (response.IsSuccessStatusCode)
            {
                Book = await response.Content.ReadFromJsonAsync<BookModel>();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.DeleteAsync($"https://localhost:32768/api/Book/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }

            return RedirectToPage("./Index"); // Handle error scenario accordingly
        }
    }
}
