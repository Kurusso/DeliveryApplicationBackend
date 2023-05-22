using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAgreagatorBackendApplication.Auth.Migrations
{
    /// <inheritdoc />
    public partial class banField2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Baned",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Baned",
                table: "AspNetUsers");
        }
    }
}
