using System.ComponentModel.DataAnnotations.Schema;

namespace BookHubAPI.Models
{
    public class BookDTO
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string ISBN { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string PublishDate { get; set; }
        public int PageCount { get; set; }
        public double Rating { get; set; }
    }
}
