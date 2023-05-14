using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAgreagatorBackendApplication.Migrations
{
    /// <inheritdoc />
    public partial class OnDeleteCookSetNullForOrder2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Cooks_CookId",
                table: "Orders",
                column: "CookId",
                principalTable: "Cooks",
                principalColumn: "Id");
        }
    }
}
