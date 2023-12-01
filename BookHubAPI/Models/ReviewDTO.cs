using System.ComponentModel.DataAnnotations.Schema;

namespace BookHubAPI.Models
{
    public class ReviewDTO
    {
     
        public string RevId { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

    }
}
