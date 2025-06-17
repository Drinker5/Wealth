using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ExchangeRates_TargetCurrencyId",
                table: "ExchangeRates",
                column: "TargetCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_BaseCurrencyId",
                table: "ExchangeRates",
                column: "BaseCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRates_Currencies_TargetCurrencyId",
                table: "ExchangeRates",
                column: "TargetCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_BaseCurrencyId",
                table: "ExchangeRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRates_Currencies_TargetCurrencyId",
                table: "ExchangeRates");

            migrationBuilder.DropIndex(
                name: "IX_ExchangeRates_TargetCurrencyId",
                table: "ExchangeRates");
        }
    }
}
