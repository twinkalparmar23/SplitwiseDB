using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDB.Migrations
{
    public partial class ninthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PayerId = table.Column<int>(nullable: false),
                    payersIdUserId = table.Column<int>(nullable: true),
                    ReceiverId = table.Column<int>(nullable: false),
                    receiversIdUserId = table.Column<int>(nullable: true),
                    GroupId = table.Column<int>(nullable: false),
                    PaidAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_User_payersIdUserId",
                        column: x => x.payersIdUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_User_receiversIdUserId",
                        column: x => x.receiversIdUserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendList_FriendId",
                table: "FriendList",
                column: "FriendId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendList_UserId",
                table: "FriendList",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_GroupId",
                table: "Transaction",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_payersIdUserId",
                table: "Transaction",
                column: "payersIdUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_receiversIdUserId",
                table: "Transaction",
                column: "receiversIdUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendList_User_FriendId",
                table: "FriendList",
                column: "FriendId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FriendList_User_UserId",
                table: "FriendList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FriendList_User_FriendId",
                table: "FriendList");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendList_User_UserId",
                table: "FriendList");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_FriendList_FriendId",
                table: "FriendList");

            migrationBuilder.DropIndex(
                name: "IX_FriendList_UserId",
                table: "FriendList");
        }
    }
}
