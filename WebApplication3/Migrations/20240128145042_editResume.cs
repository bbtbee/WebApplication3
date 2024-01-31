using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication3.Migrations
{
    /// <inheritdoc />
    public partial class editResume : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.AddColumn<byte[]>(
				name: "Resume",
				table: "AspNetUsers",
				type: "varbinary(max)",
				nullable: true);
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
				name: "ResumeFileContent",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "ResumeFileName",
				table: "AspNetUsers");
		}
    }
}
