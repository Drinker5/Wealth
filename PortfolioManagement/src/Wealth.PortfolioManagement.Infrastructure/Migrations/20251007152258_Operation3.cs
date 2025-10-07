using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Operation3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashOperation_Type",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Money_Amount",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Money_CurrencyId",
                table: "InstrumentOperations");

            migrationBuilder.RenameColumn(
                name: "CashOperation_PortfolioId",
                table: "InstrumentOperations",
                newName: "StockDividendTaxOperation_PortfolioId");

            migrationBuilder.AddColumn<int>(
                name: "BondAmortizationOperation_BondId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BondAmortizationOperation_PortfolioId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockDividendTaxOperation_StockId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BondAmortizationOperation_BondId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "BondAmortizationOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockDividendTaxOperation_StockId",
                table: "InstrumentOperations");

            migrationBuilder.RenameColumn(
                name: "StockDividendTaxOperation_PortfolioId",
                table: "InstrumentOperations",
                newName: "CashOperation_PortfolioId");

            migrationBuilder.AddColumn<byte>(
                name: "CashOperation_Type",
                table: "InstrumentOperations",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Money_Amount",
                table: "InstrumentOperations",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Money_CurrencyId",
                table: "InstrumentOperations",
                type: "smallint",
                nullable: true);
        }
    }
}
