using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForgeOps.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicle_Fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "VehicleModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "VehicleModel",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
