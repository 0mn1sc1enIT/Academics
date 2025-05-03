namespace Academics.WebAPI.Models
{
	public class TeacherDto
	{
		public int Id { get; set; }
		public string FullName { get; set; }
		public string? Description { get; set; }
		public byte[]? Photo { get; set; }

		public PositionDto Position { get; set; }
		public List<CourseSummaryDto> Courses { get; set; } = new List<CourseSummaryDto>();
	}
}
