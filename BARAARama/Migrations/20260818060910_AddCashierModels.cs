using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARAARama.Migrations
{
    /// <inheritdoc />
    public partial class AddCashierModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cashiers",
                columns: table => new
                {
                    CashierId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashierNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CashierName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashiers", x => x.CashierId);
                });

            migrationBuilder.CreateTable(
                name: "CashierWithdrawals",
                columns: table => new
                {
                    CashierWithdrawalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    MaterialName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashierId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierWithdrawals", x => x.CashierWithdrawalId);
                    table.ForeignKey(
                        name: "FK_CashierWithdrawals_Cashiers_CashierId",
                        column: x => x.CashierId,
                        principalTable: "Cashiers",
                        principalColumn: "CashierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashierWithdrawals_CashierId",
                table: "CashierWithdrawals",
                column: "CashierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashierWithdrawals");

            migrationBuilder.DropTable(
                name: "Cashiers");
        }
    }
}
