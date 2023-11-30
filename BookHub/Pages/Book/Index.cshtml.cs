using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookHub.Pages.Model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json; // Update with the correct namespace for BookModel

namespace BookHub.Pages.Book
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;

        public IndexModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public List<BookModel> Books { get; set; } = new List<BookModel>();

        public async Task OnGetAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "getitems"); // API endpoint URL
            var client = _clientFactory.CreateClient("BookApiClient");

            try
            {
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Books = JsonConvert.DeserializeObject<List<BookModel>>(content);
                }
                else
                {
                    // Handle API error response here
                    // For example, log the error or display a message
                    Books = new List<BookModel>();
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                // Log the exception or set Books to an empty list or display an error message
                Books = new List<BookModel>();
            }
        }
    }
}
