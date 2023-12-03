using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookHubAPI.Models
{
    public class Review
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Review : {Description}";
        }
    }
}
