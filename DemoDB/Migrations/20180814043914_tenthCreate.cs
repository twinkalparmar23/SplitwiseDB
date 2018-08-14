using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDB.Migrations
{
    public partial class tenthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_payersIdUserId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_receiversIdUserId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "receiversIdUserId",
                table: "Transaction",
                newName: "receiversUserId");

            migrationBuilder.RenameColumn(
                name: "payersIdUserId",
                table: "Transaction",
                newName: "payersUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_receiversIdUserId",
                table: "Transaction",
                newName: "IX_Transaction_receiversUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_payersIdUserId",
                table: "Transaction",
                newName: "IX_Transaction_payersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_payersUserId",
                table: "Transaction",
                column: "payersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_receiversUserId",
                table: "Transaction",
                column: "receiversUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_payersUserId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_receiversUserId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "receiversUserId",
                table: "Transaction",
                newName: "receiversIdUserId");

            migrationBuilder.RenameColumn(
                name: "payersUserId",
                table: "Transaction",
                newName: "payersIdUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_receiversUserId",
                table: "Transaction",
                newName: "IX_Transaction_receiversIdUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_payersUserId",
                table: "Transaction",
                newName: "IX_Transaction_payersIdUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_payersIdUserId",
                table: "Transaction",
                column: "payersIdUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_User_receiversIdUserId",
                table: "Transaction",
                column: "receiversIdUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
