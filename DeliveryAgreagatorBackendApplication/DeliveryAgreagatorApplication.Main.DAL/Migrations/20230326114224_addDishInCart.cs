using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryAgreagatorBackendApplication.Migrations
{
    /// <inheritdoc />
    public partial class addDishInCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishInCartDbModel_Customers_CustomerId",
                table: "DishInCartDbModel");

            migrationBuilder.DropForeignKey(
                name: "FK_DishInCartDbModel_Dishes_DishId",
                table: "DishInCartDbModel");

            migrationBuilder.DropForeignKey(
                name: "FK_DishInCartDbModel_Orders_OrderId",
                table: "DishInCartDbModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInCartDbModel",
                table: "DishInCartDbModel");

            migrationBuilder.RenameTable(
                name: "DishInCartDbModel",
                newName: "DishInCart");

            migrationBuilder.RenameIndex(
                name: "IX_DishInCartDbModel_OrderId",
                table: "DishInCart",
                newName: "IX_DishInCart_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_DishInCartDbModel_DishId",
                table: "DishInCart",
                newName: "IX_DishInCart_DishId");

            migrationBuilder.RenameIndex(
                name: "IX_DishInCartDbModel_CustomerId",
                table: "DishInCart",
                newName: "IX_DishInCart_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInCart",
                table: "DishInCart",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCart_Customers_CustomerId",
                table: "DishInCart",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCart_Dishes_DishId",
                table: "DishInCart",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCart_Orders_OrderId",
                table: "DishInCart",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DishInCart_Customers_CustomerId",
                table: "DishInCart");

            migrationBuilder.DropForeignKey(
                name: "FK_DishInCart_Dishes_DishId",
                table: "DishInCart");

            migrationBuilder.DropForeignKey(
                name: "FK_DishInCart_Orders_OrderId",
                table: "DishInCart");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DishInCart",
                table: "DishInCart");

            migrationBuilder.RenameTable(
                name: "DishInCart",
                newName: "DishInCartDbModel");

            migrationBuilder.RenameIndex(
                name: "IX_DishInCart_OrderId",
                table: "DishInCartDbModel",
                newName: "IX_DishInCartDbModel_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_DishInCart_DishId",
                table: "DishInCartDbModel",
                newName: "IX_DishInCartDbModel_DishId");

            migrationBuilder.RenameIndex(
                name: "IX_DishInCart_CustomerId",
                table: "DishInCartDbModel",
                newName: "IX_DishInCartDbModel_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DishInCartDbModel",
                table: "DishInCartDbModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCartDbModel_Customers_CustomerId",
                table: "DishInCartDbModel",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCartDbModel_Dishes_DishId",
                table: "DishInCartDbModel",
                column: "DishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DishInCartDbModel_Orders_OrderId",
                table: "DishInCartDbModel",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
