namespace Academics.Admin.Models
{
	public class Teacher
	{
		public int Id { get; set; }
		public DateTime CreatedAt { get; set; }
		public string CreatedBy { get; set; }

		public string FullName { get; set; }
		public byte[]? Photo { get; set; }
		public string Description { get; set; }

		public int PositionId { get; set; }
		public Position Position { get; set; }

		public ICollection<Course> Courses { get; set; }
	}
}
