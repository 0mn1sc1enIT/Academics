namespace Academics.Models
{
	public class Teacher
	{
		public int Id { get; set; }
		public DateTime? CreatedAt { get; set; } = DateTime.Now;
		public string? CreatedBy { get; set; } = "admin";

		public string FullName { get; set; }
		public byte[]? Photo { get; set; }
		public string? Description { get; set; }

		public int PositionId { get; set; }
		public Position Position { get; set; }

		public ICollection<Course>? Courses { get; set; }
	}
}
