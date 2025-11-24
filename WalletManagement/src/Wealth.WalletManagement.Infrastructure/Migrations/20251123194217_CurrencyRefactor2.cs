using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.WalletManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount_CurrencyId",
                table: "WalletOperations",
                newName: "Amount_Currency");

            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "WalletCurrency",
                newName: "Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                table: "WalletOperations",
                newName: "Amount_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "WalletCurrency",
                newName: "CurrencyId");
        }
    }
}
