using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookHubAPI.Models
{
    public class ReviewDTO
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
      
    }
}
