using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "TargetCurrencyId", table: "ExchangeRates");
            migrationBuilder.AddColumn<byte>(
                name: "TargetCurrencyId",
                table: "ExchangeRates",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "BaseCurrencyId", table: "ExchangeRates");
            migrationBuilder.AddColumn<byte>(
                name: "BaseCurrencyId",
                table: "ExchangeRates",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "Id", table: "Currencies");
            migrationBuilder.AddColumn<byte>(
                name: "Id",
                table: "Currencies",
                type: "smallint",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TargetCurrencyId",
                table: "ExchangeRates",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "BaseCurrencyId",
                table: "ExchangeRates",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Currencies",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
