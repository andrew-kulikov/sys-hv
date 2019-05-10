using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class RemoveMinAndMaxValuesFromClientSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxValue",
                table: "ClientSensors");

            migrationBuilder.DropColumn(
                name: "MinValue",
                table: "ClientSensors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "MaxValue",
                table: "ClientSensors",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MinValue",
                table: "ClientSensors",
                nullable: true);
        }
    }
}
