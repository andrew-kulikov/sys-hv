using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class ExtendSensorModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnType",
                table: "SensorTypes");

            migrationBuilder.AddColumn<float>(
                name: "CriticalValue",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumeric",
                table: "Sensors",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "MaxValue",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MinValue",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnType",
                table: "Sensors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "IsNumeric",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "ReturnType",
                table: "Sensors");

            migrationBuilder.AddColumn<string>(
                name: "ReturnType",
                table: "SensorTypes",
                nullable: true);
        }
    }
}
