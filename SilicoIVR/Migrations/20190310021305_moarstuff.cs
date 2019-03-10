using Microsoft.EntityFrameworkCore.Migrations;

namespace SilicoIVR.Migrations
{
    public partial class moarstuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Transcription",
                table: "Recordings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Transcription",
                table: "Recordings");
        }
    }
}
