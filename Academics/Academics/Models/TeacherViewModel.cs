namespace Academics.Models
{
    public class TeacherViewModel
    {
		public int Id { get; set; }
		public string FullName { get; set; } = string.Empty;
		public string? Description { get; set; }
		public string PositionName { get; set; } = string.Empty;
		public string? PhotoBase64 { get; set; }
	}
}
