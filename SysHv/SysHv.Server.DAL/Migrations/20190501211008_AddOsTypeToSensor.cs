using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class AddOsTypeToSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OsType",
                table: "Sensors",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OsType",
                table: "Sensors");
        }
    }
}
