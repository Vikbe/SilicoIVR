using Microsoft.EntityFrameworkCore.Migrations;

namespace SilicoIVR.Migrations
{
    public partial class morecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Calls",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Calls");
        }
    }
}
