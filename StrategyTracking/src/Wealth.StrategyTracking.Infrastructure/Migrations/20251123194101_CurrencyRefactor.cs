using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.StrategyTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "StrategyComponent",
                newName: "Currency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "StrategyComponent",
                newName: "CurrencyId");
        }
    }
}
