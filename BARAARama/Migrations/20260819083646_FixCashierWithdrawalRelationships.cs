using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BARAARama.Migrations
{
    /// <inheritdoc />
    public partial class FixCashierWithdrawalRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CashierWithdrawals_MaterialId",
                table: "CashierWithdrawals",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashierWithdrawals_Materials_MaterialId",
                table: "CashierWithdrawals",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashierWithdrawals_Materials_MaterialId",
                table: "CashierWithdrawals");

            migrationBuilder.DropIndex(
                name: "IX_CashierWithdrawals_MaterialId",
                table: "CashierWithdrawals");
        }
    }
}
