using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class MoveConfiguratiionToClientSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalValue",
                table: "SubSensors");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "SubSensors");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "SubSensors");

            migrationBuilder.DropColumn(
                name: "CriticalValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "QueueName",
                table: "Clients");

            migrationBuilder.AddColumn<float>(
                name: "CriticalValue",
                table: "ClientSensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxValue",
                table: "ClientSensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MinValue",
                table: "ClientSensors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CriticalValue",
                table: "ClientSensors");

            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "ClientSensors");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "ClientSensors");

            migrationBuilder.AddColumn<float>(
                name: "CriticalValue",
                table: "SubSensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxValue",
                table: "SubSensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MinValue",
                table: "SubSensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CriticalValue",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MaxValue",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MinValue",
                table: "Sensors",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueueName",
                table: "Clients",
                nullable: true);
        }
    }
}
