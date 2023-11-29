using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace bookData
{
    public class IndustryIdentifier
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
        // Add other identifier properties if required...
    }

    public class ReadingModes
    {
        public bool Text { get; set; }
        public bool Image { get; set; }
        // Add other reading mode properties if required...
    }

    public class PanelizationSummary
    {
        public bool ContainsEpubBubbles { get; set; }
        public bool ContainsImageBubbles { get; set; }
        // Add other panelization summary properties if required...
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public string[] Authors { get; set; }
        public string Subtitle { get; set; }
        public ImageLinks ImageLinks { get; set; }
        public IndustryIdentifier[] IndustryIdentifiers { get; set; }
        public string Description { get; set; }
        public string InfoLink { get; set; }
        public string PublishedDate { get; set; }
        public int PageCount { get; set; }
        public ReadingModes ReadingModes { get; set; }
        public string PrintType { get; set; }
        public string[] Categories { get; set; }
        public double AverageRating { get; set; }
        public int RatingsCount { get; set; }
        public PanelizationSummary PanelizationSummary { get; set; }
        // Add other properties from VolumeInfo if required...
    }

    public class ImageLinks
    {
        public string SmallThumbnail { get; set; }
        public string Thumbnail { get; set; }
        // Add other image properties if required...
    }

    public class IndustryIdentifiers
    {
        public string Type { get; set; }
        public string Identifier { get; set; }
        // Add other identifier properties if required...
    }

    public class BookItem
    {
        public string Id { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
        // Add other properties if required...
    }

    public class BooksResponse
    {
        public int TotalItems { get; set; }
        public List<BookItem> Items { get; set; }
    }

    public class Program
    {

        static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            string apiKey = "AIzaSyBHtMtHH4IhP1bTCSAkmGID-vJXXSigm9c";
            int maxResults = 30;

            List<BookItem> allBooks = await FetchBooksFromApi(apiKey, maxResults);

            await StoreDataInDatabase(allBooks);
        }

        static async Task<List<BookItem>> FetchBooksFromApi(string apiKey, int maxResults)
        {
            int startIndex = 0;
            List<BookItem> allBooks = new List<BookItem>();

            while (allBooks.Count < maxResults)
            {
                string apiUrl = "https://www.googleapis.com/books/v1/volumes?q=hacking&startIndex=0&maxResults=40&key=AIzaSyBHtMtHH4IhP1bTCSAkmGID-vJXXSigm9c";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    var booksResponse = JsonConvert.DeserializeObject<BooksResponse>(json);

                    if (booksResponse?.Items != null)
                    {
                        allBooks.AddRange(booksResponse.Items);
                    }
                    else
                    {
                        Console.WriteLine("Error: No items found in the response.");
                        break;
                    }

                    startIndex += 40;

                    if (booksResponse.TotalItems == 0 || allBooks.Count >= maxResults)
                    {
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                    break;
                }
            }

            return allBooks.Take(maxResults).ToList();
        }

        static async Task StoreDataInDatabase(List<BookItem> allBooks)
        {
            using (var dbContext = new BooksDbContext())
            {
                foreach (var bookItem in allBooks)
                {

                    var existingBook = dbContext.Books.FirstOrDefault(b => b.Id == bookItem.Id);

                    if (existingBook == null)
                    {
                        var newBook = new Book
                        {
                            Id = bookItem.Id,
                            Title = bookItem.VolumeInfo?.Title ?? "Unknown Title",
                            Subtitle = bookItem.VolumeInfo?.Subtitle ?? string.Empty,
                            Author = bookItem.VolumeInfo?.Authors != null && bookItem.VolumeInfo.Authors.Length > 0
                                ? string.Join(", ", bookItem.VolumeInfo.Authors)
                                : "Unknown Author",
                            Image = bookItem.VolumeInfo?.ImageLinks?.Thumbnail ?? string.Empty,
                            ISBN = bookItem.VolumeInfo?.IndustryIdentifiers?.FirstOrDefault()?.Identifier ?? string.Empty,
                            Description = bookItem.VolumeInfo?.Description ?? string.Empty,
                            Link = bookItem.VolumeInfo?.InfoLink ?? string.Empty,
                            PublishDate = bookItem.VolumeInfo?.PublishedDate ?? string.Empty,
                            PageCount = bookItem.VolumeInfo?.PageCount ?? 0,
                            Rating = bookItem.VolumeInfo?.AverageRating ?? 0.0
                            // Map other properties if required...
                        };

                        dbContext.Books.Add(newBook);
                    }
                    else
                    {
                        // For example, you might want to update the existing book with new data
                        existingBook.Title = bookItem.VolumeInfo?.Title ?? existingBook.Title;
                        existingBook.Author = bookItem.VolumeInfo?.Authors != null && bookItem.VolumeInfo.Authors.Length > 0
                                     ? string.Join(", ", bookItem.VolumeInfo.Authors)
                                     : existingBook.Author;// Update other properties as needed
                        // dbContext.Books.Update(existingBook); // Uncomment this line if updating is required
                    }
                }
                await dbContext.SaveChangesAsync();
            }
        }
    } 
    
}
        

