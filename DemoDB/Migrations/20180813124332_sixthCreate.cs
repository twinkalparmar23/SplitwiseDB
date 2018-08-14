using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDB.Migrations
{
    public partial class sixthCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillMember",
                columns: table => new
                {
                    BillMemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Billid = table.Column<int>(nullable: false),
                    SharedMemberId = table.Column<int>(nullable: false),
                    AmountToPay = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillMember", x => x.BillMemberId);
                    table.ForeignKey(
                        name: "FK_BillMember_Bill_Billid",
                        column: x => x.Billid,
                        principalTable: "Bill",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BillMember_User_SharedMemberId",
                        column: x => x.SharedMemberId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GroupPayer",
                columns: table => new
                {
                    GroupPayerId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<int>(nullable: false),
                    PayerId = table.Column<int>(nullable: false),
                    PaidAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPayer", x => x.GroupPayerId);
                    table.ForeignKey(
                        name: "FK_GroupPayer_Bill_BillId",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupPayer_User_PayerId",
                        column: x => x.PayerId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "IndividualPayer",
                columns: table => new
                {
                    IndividualPayerid = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BillId = table.Column<int>(nullable: false),
                    PayerId = table.Column<int>(nullable: false),
                    PaidAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IndividualPayer", x => x.IndividualPayerid);
                    table.ForeignKey(
                        name: "FK_IndividualPayer_Bill_BillId",
                        column: x => x.BillId,
                        principalTable: "Bill",
                        principalColumn: "BillId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IndividualPayer_User_PayerId",
                        column: x => x.PayerId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BillMember_Billid",
                table: "BillMember",
                column: "Billid");

            migrationBuilder.CreateIndex(
                name: "IX_BillMember_SharedMemberId",
                table: "BillMember",
                column: "SharedMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPayer_BillId",
                table: "GroupPayer",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupPayer_PayerId",
                table: "GroupPayer",
                column: "PayerId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualPayer_BillId",
                table: "IndividualPayer",
                column: "BillId");

            migrationBuilder.CreateIndex(
                name: "IX_IndividualPayer_PayerId",
                table: "IndividualPayer",
                column: "PayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillMember");

            migrationBuilder.DropTable(
                name: "GroupPayer");

            migrationBuilder.DropTable(
                name: "IndividualPayer");
        }
    }
}
