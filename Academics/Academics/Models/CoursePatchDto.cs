namespace Academics.Models
{
	public class CoursePatchDto
	{
		public string? CourseTitle { get; set; }
		public string? Description { get; set; }
		public string? Hours { get; set; }
		public decimal? Price { get; set; }
		public int? Stars { get; set; }
		public int? TeacherId { get; set; }
	}
}
