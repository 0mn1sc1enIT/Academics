namespace Academics.Models
{
	public class Position
	{
		public int Id { get; set; }
		public DateTime? CreatedAt { get; set; } = DateTime.Now;
		public string? CreatedBy { get; set; } = "admin";

		public string Name { get; set; }
		public string? Description { get; set; }


		public ICollection<Teacher>? Teachers { get; set; }
	}
}
