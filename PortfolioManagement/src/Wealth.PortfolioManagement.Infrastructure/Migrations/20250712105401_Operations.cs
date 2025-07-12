using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Operations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISIN",
                table: "PortfolioAsset");

            migrationBuilder.CreateTable(
                name: "CashOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
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
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
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
                name: "SplitOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ratio_Old = table.Column<int>(type: "integer", nullable: false),
                    Ratio_New = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SplitOperations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TradeOperations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    InstrumentId = table.Column<Guid>(type: "uuid", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false),
                    Money_CurrencyId = table.Column<string>(type: "text", nullable: false),
                    Money_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeOperations", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashOperations");

            migrationBuilder.DropTable(
                name: "CurrencyOperations");

            migrationBuilder.DropTable(
                name: "DelistOperations");

            migrationBuilder.DropTable(
                name: "SplitOperations");

            migrationBuilder.DropTable(
                name: "TradeOperations");

            migrationBuilder.AddColumn<string>(
                name: "ISIN",
                table: "PortfolioAsset",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "");
        }
    }
}
