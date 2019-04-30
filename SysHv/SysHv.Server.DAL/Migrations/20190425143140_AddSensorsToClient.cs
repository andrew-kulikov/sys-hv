using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class AddSensorsToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminId",
                table: "Sensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Sensors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Sensors",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_ClientId",
                table: "Sensors",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_UserId",
                table: "Sensors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Clients_ClientId",
                table: "Sensors",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_AspNetUsers_UserId",
                table: "Sensors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Clients_ClientId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_AspNetUsers_UserId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_ClientId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_UserId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Sensors");
        }
    }
}
