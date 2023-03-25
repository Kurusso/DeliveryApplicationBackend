using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAgreagatorBackendApplication.Migrations
{
    /// <inheritdoc />
    public partial class MenuModelChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isOld",
                table: "Menus",
                newName: "isActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isActive",
                table: "Menus",
                newName: "isOld");
        }
    }
}
