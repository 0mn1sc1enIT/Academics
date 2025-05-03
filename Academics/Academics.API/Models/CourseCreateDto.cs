using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Academics.WebAPI.Models
{
	public class CourseCreateDto
	{
		public int TeacherId { get; set; }

		public string Hours { get; set; }

		public string? Description { get; set; }
		public byte[]? Image { get; set; }

		public string CourseTitle { get; set; }

		[Column(TypeName = "decimal(18, 2)")]

		public decimal Price { get; set; }

		[Range(0, 5)]
		public int Stars { get; set; }
		public string Category { get; set; }

	}
}
