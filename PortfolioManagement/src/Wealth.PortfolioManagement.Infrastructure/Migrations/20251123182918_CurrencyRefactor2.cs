using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrencyId",
                table: "PortfolioCurrency",
                newName: "Currency");

            migrationBuilder.RenameColumn(
                name: "Amount_CurrencyId",
                table: "InstrumentOperations",
                newName: "Amount_Currency");

            migrationBuilder.CreateTable(
                name: "CurrencyAsset",
                columns: table => new
                {
                    CurrencyId = table.Column<byte>(type: "smallint", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyAsset", x => new { x.PortfolioId, x.CurrencyId });
                    table.ForeignKey(
                        name: "FK_CurrencyAsset_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrencyAsset");

            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "PortfolioCurrency",
                newName: "CurrencyId");

            migrationBuilder.RenameColumn(
                name: "Amount_Currency",
                table: "InstrumentOperations",
                newName: "Amount_CurrencyId");
        }
    }
}
