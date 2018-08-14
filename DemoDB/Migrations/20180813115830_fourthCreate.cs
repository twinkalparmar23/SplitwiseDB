using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDB.Migrations
{
    public partial class fourthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bill_CreatorId",
                table: "Bill",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bill_User_CreatorId",
                table: "Bill",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bill_User_CreatorId",
                table: "Bill");

            migrationBuilder.DropIndex(
                name: "IX_Bill_CreatorId",
                table: "Bill");
        }
    }
}
