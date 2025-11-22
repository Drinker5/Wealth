using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.RenameColumn(
                name: "TargetCurrencyId",
                table: "ExchangeRates",
                newName: "TargetCurrency");

            migrationBuilder.RenameColumn(
                name: "BaseCurrencyId",
                table: "ExchangeRates",
                newName: "BaseCurrency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetCurrency",
                table: "ExchangeRates",
                newName: "TargetCurrencyId");

            migrationBuilder.RenameColumn(
                name: "BaseCurrency",
                table: "ExchangeRates",
                newName: "BaseCurrencyId");

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Symbol = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

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
    }
}
