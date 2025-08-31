using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashOperations");

            migrationBuilder.DropTable(
                name: "CurrencyOperations");

            migrationBuilder.DropTable(
                name: "DelistOperations");

            migrationBuilder.DropTable(
                name: "PortfolioAsset");

            migrationBuilder.DropTable(
                name: "TradeOperations");

            migrationBuilder.DropTable(
                name: "SplitOperations");

            migrationBuilder.CreateTable(
                name: "InstrumentOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<int>(type: "integer", nullable: true),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: true),
                    Ratio_Old = table.Column<int>(type: "integer", nullable: true),
                    Ratio_New = table.Column<int>(type: "integer", nullable: true),
                    BondId = table.Column<int>(type: "integer", nullable: true),
                    BondTradeOperation_PortfolioId = table.Column<int>(type: "integer", nullable: true),
                    BondTradeOperation_Type = table.Column<int>(type: "integer", nullable: true),
                    CashOperation_PortfolioId = table.Column<int>(type: "integer", nullable: true),
                    CashOperation_Type = table.Column<int>(type: "integer", nullable: true),
                    StockId = table.Column<int>(type: "integer", nullable: true),
                    StockTradeOperation_PortfolioId = table.Column<int>(type: "integer", nullable: true),
                    StockTradeOperation_StockId = table.Column<int>(type: "integer", nullable: true),
                    StockTradeOperation_Type = table.Column<int>(type: "integer", nullable: true),
                    operation_type = table.Column<byte>(type: "smallint", nullable: false, defaultValue: (byte)0),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BondAsset",
                columns: table => new
                {
                    BondId = table.Column<int>(type: "integer", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BondAsset", x => new { x.PortfolioId, x.BondId });
                    table.ForeignKey(
                        name: "FK_BondAsset_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAsset",
                columns: table => new
                {
                    StockId = table.Column<int>(type: "integer", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAsset", x => new { x.PortfolioId, x.StockId });
                    table.ForeignKey(
                        name: "FK_StockAsset_Portfolios_PortfolioId",
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
                name: "BondAsset");

            migrationBuilder.DropTable(
                name: "StockAsset");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InstrumentOperations",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "BondId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "BondTradeOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "BondTradeOperation_Type",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "CashOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "CashOperation_Type",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Money_Amount",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Money_CurrencyId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockTradeOperation_PortfolioId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockTradeOperation_StockId",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "StockTradeOperation_Type",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "operation_type",
                table: "InstrumentOperations");

            migrationBuilder.RenameTable(
                name: "InstrumentOperations",
                newName: "SplitOperations");

            migrationBuilder.AlterColumn<int>(
                name: "Ratio_Old",
                table: "SplitOperations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Ratio_New",
                table: "SplitOperations",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InstrumentId",
                table: "SplitOperations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_SplitOperations",
                table: "SplitOperations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CashOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrencyOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrencyOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DelistOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelistOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioAsset",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioAsset", x => new { x.PortfolioId, x.InstrumentId });
                    table.ForeignKey(
                        name: "FK_PortfolioAsset_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeOperations", x => x.Id);
                });
        }
    }
}
