using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "CurrencyId", table: "PortfolioCurrency");
            migrationBuilder.AddColumn<byte>(
                name: "CurrencyId",
                table: "PortfolioCurrency",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "Money_CurrencyId", table: "InstrumentOperations");
            migrationBuilder.AddColumn<byte>(
                name: "Money_CurrencyId",
                table: "InstrumentOperations",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "PortfolioCurrency",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Money_CurrencyId",
                table: "InstrumentOperations",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldNullable: true);
        }
    }
}
