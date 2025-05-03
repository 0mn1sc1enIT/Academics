using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Academics.Admin.Migrations
{
    /// <inheritdoc />
    public partial class addImageForCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "image",
                table: "Courses",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Courses");
        }
    }
}
