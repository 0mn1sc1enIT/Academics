using Microsoft.EntityFrameworkCore;

namespace Academics.WebAPI.Models
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options) { }

		public DbSet<Position> Positions { get; set; }
		public DbSet<Teacher> Teachers { get; set; }
		public DbSet<Course> Courses { get; set; }
	}
}
