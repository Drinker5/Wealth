using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.DepositManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Investment_CurrencyId",
                table: "Deposits",
                newName: "Investment_Currency");

            migrationBuilder.RenameColumn(
                name: "Money_CurrencyId",
                table: "DepositOperations",
                newName: "Money_Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Investment_Currency",
                table: "Deposits",
                newName: "Investment_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Money_Currency",
                table: "DepositOperations",
                newName: "Money_CurrencyId");
        }
    }
}
