using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookHubAPI.Models
{
    public class Review
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RevId { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $"ID : {RevId}, Review : {Description}, Book-Id: {Id}";
        }
    }
}
