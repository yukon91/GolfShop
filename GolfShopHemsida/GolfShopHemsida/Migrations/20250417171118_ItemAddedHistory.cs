using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfShopHemsida.Migrations
{
    /// <inheritdoc />
    public partial class ItemAddedHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderItemId1",
                table: "OrderItems",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderItemId1",
                table: "OrderItems",
                column: "OrderItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_OrderItems_OrderItemId1",
                table: "OrderItems",
                column: "OrderItemId1",
                principalTable: "OrderItems",
                principalColumn: "OrderItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_OrderItems_OrderItemId1",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderItemId1",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "OrderItemId1",
                table: "OrderItems");
        }
    }
}
