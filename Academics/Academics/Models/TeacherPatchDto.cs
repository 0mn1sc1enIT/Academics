namespace Academics.Models
{
	public class TeacherPatchDto
	{
		public string? FullName { get; set; }
		public string? Description { get; set; }
		public byte[]? Photo { get; set; }
		public int? PositionId { get; set; } 
	}
}
