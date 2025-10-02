using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Operation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BondTradeOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "BondTradeOperation_Type",
                table: "InstrumentOperations");

            migrationBuilder.RenameColumn(
                name: "StockTradeOperation_Type",
                table: "InstrumentOperations",
                newName: "TradeOperation_Type");

            migrationBuilder.RenameColumn(
                name: "StockTradeOperation_PortfolioId",
                table: "InstrumentOperations",
                newName: "TradeOperation_PortfolioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TradeOperation_Type",
                table: "InstrumentOperations",
                newName: "StockTradeOperation_Type");

            migrationBuilder.RenameColumn(
                name: "TradeOperation_PortfolioId",
                table: "InstrumentOperations",
                newName: "StockTradeOperation_PortfolioId");

            migrationBuilder.AddColumn<int>(
                name: "BondTradeOperation_PortfolioId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BondTradeOperation_Type",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);
        }
    }
}
