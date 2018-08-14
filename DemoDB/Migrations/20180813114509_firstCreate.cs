using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDB.Migrations
{
    public partial class firstCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_Group_GroupId1",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_Group_GroupId",
                table: "GroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_User_UserId",
                table: "GroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_User_User_UserId1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_UserId1",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_GroupMember_GroupId",
                table: "GroupMember");

            migrationBuilder.DropIndex(
                name: "IX_GroupMember_UserId",
                table: "GroupMember");

            migrationBuilder.DropIndex(
                name: "IX_Group_GroupId1",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "User");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Group");

            migrationBuilder.AddColumn<int>(
                name: "Group_Id",
                table: "GroupMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "User_Id",
                table: "GroupMember",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_Group_Id",
                table: "GroupMember",
                column: "Group_Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_User_Id",
                table: "GroupMember",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_Group_Group_Id",
                table: "GroupMember",
                column: "Group_Id",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_User_User_Id",
                table: "GroupMember",
                column: "User_Id",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_Group_Group_Id",
                table: "GroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_User_User_Id",
                table: "GroupMember");

            migrationBuilder.DropIndex(
                name: "IX_GroupMember_Group_Id",
                table: "GroupMember");

            migrationBuilder.DropIndex(
                name: "IX_GroupMember_User_Id",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "Group_Id",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "User_Id",
                table: "GroupMember");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "GroupMember",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "GroupMember",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "Group",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserId1",
                table: "User",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId",
                table: "GroupMember",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_UserId",
                table: "GroupMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Group_GroupId1",
                table: "Group",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Group_GroupId1",
                table: "Group",
                column: "GroupId1",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_Group_GroupId",
                table: "GroupMember",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_User_UserId",
                table: "GroupMember",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_User_UserId1",
                table: "User",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
