using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class AddClientAndUserEmailToSensorLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "SensorLogs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserEmail",
                table: "SensorLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "SensorLogs");

            migrationBuilder.DropColumn(
                name: "UserEmail",
                table: "SensorLogs");
        }
    }
}
