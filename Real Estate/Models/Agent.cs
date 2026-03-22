using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Real_Estate.Models
{
	public class Agent
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Name")]
		[Required]
		public string? Name { get; set; }
		
		[DisplayName("Location")]
		[Required]
		public string? Location { get; set; }
		[DisplayName("Image")]

		public string? Path { get; set; }
		[NotMapped]
		[DisplayName("Choose image")]
		public IFormFile Image { get; set; }
	}
}
