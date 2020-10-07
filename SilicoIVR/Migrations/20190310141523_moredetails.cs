using Microsoft.EntityFrameworkCore.Migrations;

namespace SilicoIVR.Migrations
{
    public partial class moredetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recordings_Calls_CallID",
                table: "Recordings");

            migrationBuilder.AlterColumn<int>(
                name: "CallID",
                table: "Recordings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "AgentCalledID",
                table: "Calls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CallerType",
                table: "Calls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Calls",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Calls_AgentCalledID",
                table: "Calls",
                column: "AgentCalledID");

            migrationBuilder.AddForeignKey(
                name: "FK_Calls_Agents_AgentCalledID",
                table: "Calls",
                column: "AgentCalledID",
                principalTable: "Agents",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Recordings_Calls_CallID",
                table: "Recordings",
                column: "CallID",
                principalTable: "Calls",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calls_Agents_AgentCalledID",
                table: "Calls");

            migrationBuilder.DropForeignKey(
                name: "FK_Recordings_Calls_CallID",
                table: "Recordings");

            migrationBuilder.DropIndex(
                name: "IX_Calls_AgentCalledID",
                table: "Calls");

            migrationBuilder.DropColumn(
                name: "AgentCalledID",
                table: "Calls");

            migrationBuilder.DropColumn(
                name: "CallerType",
                table: "Calls");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Calls");

            migrationBuilder.AlterColumn<int>(
                name: "CallID",
                table: "Recordings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recordings_Calls_CallID",
                table: "Recordings",
                column: "CallID",
                principalTable: "Calls",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
