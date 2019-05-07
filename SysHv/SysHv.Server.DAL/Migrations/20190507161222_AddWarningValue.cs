using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class AddWarningValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "WarningValue",
                table: "ClientSensors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WarningValue",
                table: "ClientSensors");
        }
    }
}
