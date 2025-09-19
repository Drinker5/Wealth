using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.Aggregation.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "TotalInvestments_CurrencyId", table: "StockAggregations");
            migrationBuilder.AddColumn<byte>(
                name: "TotalInvestments_CurrencyId",
                table: "StockAggregations",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "TotalDividends_CurrencyId", table: "StockAggregations");
            migrationBuilder.AddColumn<byte>(
                name: "TotalDividends_CurrencyId",
                table: "StockAggregations",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "StockPrice_CurrencyId", table: "StockAggregations");
            migrationBuilder.AddColumn<byte>(
                name: "StockPrice_CurrencyId",
                table: "StockAggregations",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "DividendPerYear_CurrencyId", table: "StockAggregations");
            migrationBuilder.AddColumn<byte>(
                name: "DividendPerYear_CurrencyId",
                table: "StockAggregations",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "Id", table: "StockAggregations");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StockAggregations",
                type: "integer",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false),
                    OccurredOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProcessedOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages");

            migrationBuilder.AlterColumn<string>(
                name: "TotalInvestments_CurrencyId",
                table: "StockAggregations",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "TotalDividends_CurrencyId",
                table: "StockAggregations",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "StockPrice_CurrencyId",
                table: "StockAggregations",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "DividendPerYear_CurrencyId",
                table: "StockAggregations",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StockAggregations",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
