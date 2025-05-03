namespace Academics.Models
{
	public class CourseGetDto
	{
		public int Id { get; set; }
		public string CourseTitle { get; set; }
		public string? Description { get; set; }
		public string Hours { get; set; }
		public decimal Price { get; set; }
		public int Stars { get; set; }
		public int TeacherId { get; set; }
		public string TeacherName { get; set; }
		public byte[]? Image { get; set; }
		public string Category { get; set; }
	}
}
