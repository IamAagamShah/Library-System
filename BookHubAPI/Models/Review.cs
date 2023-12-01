using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookHubAPI.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RevId { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }

        public override string ToString()
        {
            return $"ID : {RevId}, Review : {Description}, Book-Id: {Id}";
        }
    }
}
