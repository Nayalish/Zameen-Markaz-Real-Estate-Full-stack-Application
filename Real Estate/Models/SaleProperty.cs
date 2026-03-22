using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate.Models
{
    public class SaleProperty
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Author Name")]
        [Required]
        public string? Author_Name { get; set; }

        [DisplayName("Property Name")]
        [Required]
        public string? Property_Name { get; set; }
        [DisplayName("Location")]
        [Required]
        public string? Location { get; set; }
        [DisplayName("Price")]
        [Required]
        public string? Price { get; set; }
        
        [DisplayName("Image")]

        public string? Path { get; set; }
        [NotMapped]
        [DisplayName("Choose image")]
        public IFormFile Image { get; set; }
    }
}
