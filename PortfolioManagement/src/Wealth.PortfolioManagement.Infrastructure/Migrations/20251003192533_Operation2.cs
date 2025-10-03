using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Operation2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Type",
                table: "InstrumentOperations",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "CashOperation_Type",
                table: "InstrumentOperations",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BondCouponOperation_BondId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BondCouponOperation_PortfolioId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockDividendOperation_PortfolioId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockDividendOperation_StockId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockSplitOperation_StockId",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BondCouponOperation_BondId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "BondCouponOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockDividendOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockDividendOperation_StockId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockSplitOperation_StockId",
                table: "InstrumentOperations");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CashOperation_Type",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldNullable: true);
        }
    }
}
