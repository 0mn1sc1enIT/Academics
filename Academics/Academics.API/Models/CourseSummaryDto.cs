﻿namespace Academics.WebAPI.Models
{
	public class CourseSummaryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
	}
}
