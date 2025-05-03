namespace Academics.Models
{
	public class CourseViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public int Stars { get; set; } // 0-5
		public string TeacherName { get; set; }
		public string Hours { get; set; }

		public byte[]? Image { get; set; }
		public string Category { get; set; } = "Категория";
		public string CourseUrl { get; set; }
	}
}
