using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForgeOps.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicle_Fix_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleModel_Customers_CurrentOwnerId",
                table: "VehicleModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleModel",
                table: "VehicleModel");

            migrationBuilder.RenameTable(
                name: "VehicleModel",
                newName: "Vehicles");

            migrationBuilder.RenameIndex(
                name: "IX_VehicleModel_CurrentOwnerId",
                table: "Vehicles",
                newName: "IX_Vehicles_CurrentOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Customers_CurrentOwnerId",
                table: "Vehicles",
                column: "CurrentOwnerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Customers_CurrentOwnerId",
                table: "Vehicles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vehicles",
                table: "Vehicles");

            migrationBuilder.RenameTable(
                name: "Vehicles",
                newName: "VehicleModel");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_CurrentOwnerId",
                table: "VehicleModel",
                newName: "IX_VehicleModel_CurrentOwnerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleModel",
                table: "VehicleModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleModel_Customers_CurrentOwnerId",
                table: "VehicleModel",
                column: "CurrentOwnerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
