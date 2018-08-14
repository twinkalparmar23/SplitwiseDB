using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDB.Migrations
{
    public partial class fifthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Group_CreatorId",
                table: "Group",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_User_CreatorId",
                table: "Group",
                column: "CreatorId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_User_CreatorId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_Group_CreatorId",
                table: "Group");
        }
    }
}
