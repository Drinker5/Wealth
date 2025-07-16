using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.Aggregation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price_CurrencyId",
                table: "StockAggregations",
                newName: "StockPrice_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Price_Amount",
                table: "StockAggregations",
                newName: "StockPrice_Amount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StockPrice_CurrencyId",
                table: "StockAggregations",
                newName: "Price_CurrencyId");

            migrationBuilder.RenameColumn(
                name: "StockPrice_Amount",
                table: "StockAggregations",
                newName: "Price_Amount");
        }
    }
}
