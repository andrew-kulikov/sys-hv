using Microsoft.EntityFrameworkCore.Migrations;

namespace SysHv.Server.DAL.Migrations
{
    public partial class RenameSubSensors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubSensor_Sensors_SensorId",
                table: "SubSensor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubSensor",
                table: "SubSensor");

            migrationBuilder.RenameTable(
                name: "SubSensor",
                newName: "SubSensors");

            migrationBuilder.RenameIndex(
                name: "IX_SubSensor_SensorId",
                table: "SubSensors",
                newName: "IX_SubSensors_SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubSensors",
                table: "SubSensors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubSensors_Sensors_SensorId",
                table: "SubSensors",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubSensors_Sensors_SensorId",
                table: "SubSensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubSensors",
                table: "SubSensors");

            migrationBuilder.RenameTable(
                name: "SubSensors",
                newName: "SubSensor");

            migrationBuilder.RenameIndex(
                name: "IX_SubSensors_SensorId",
                table: "SubSensor",
                newName: "IX_SubSensor_SensorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubSensor",
                table: "SubSensor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubSensor_Sensors_SensorId",
                table: "SubSensor",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
