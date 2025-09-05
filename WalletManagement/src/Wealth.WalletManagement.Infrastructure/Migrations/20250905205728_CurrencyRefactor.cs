using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.WalletManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Amount_CurrencyId", table: "WalletOperations");
            migrationBuilder.AddColumn<byte>(
                name: "Amount_CurrencyId",
                table: "WalletOperations",
                type: "smallint",
                maxLength: 3,
                nullable: false);

            migrationBuilder.DropColumn(name: "CurrencyId", table: "WalletCurrency");
            migrationBuilder.AddColumn<byte>(
                name: "CurrencyId",
                table: "WalletCurrency",
                type: "smallint",
                maxLength: 3,
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Amount_CurrencyId",
                table: "WalletOperations",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldMaxLength: 3);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "WalletCurrency",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldMaxLength: 3);
        }
    }
}
