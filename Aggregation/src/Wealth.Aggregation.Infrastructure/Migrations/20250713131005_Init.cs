using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.Aggregation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockAggregations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Price_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DividendPerYear_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    DividendPerYear_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    LotSize = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    TotalInvestments_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    TotalInvestments_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalDividends_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    TotalDividends_Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAggregations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockAggregations");
        }
    }
}
