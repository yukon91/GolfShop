using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GolfShopHemsida.Migrations
{
    /// <inheritdoc />
    public partial class FixedUserPostRelationshoip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_GolfShopUserId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "GolfShopUserId1",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_GolfShopUserId1",
                table: "Posts",
                column: "GolfShopUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_GolfShopUserId",
                table: "Comments",
                column: "GolfShopUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_GolfShopUserId1",
                table: "Posts",
                column: "GolfShopUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_AspNetUsers_GolfShopUserId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_GolfShopUserId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_GolfShopUserId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "GolfShopUserId1",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_AspNetUsers_GolfShopUserId",
                table: "Comments",
                column: "GolfShopUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
