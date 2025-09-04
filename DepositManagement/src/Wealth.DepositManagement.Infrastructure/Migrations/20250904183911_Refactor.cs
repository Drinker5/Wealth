using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.DepositManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "Investment_CurrencyId", table: "Deposits");
            migrationBuilder.AddColumn<byte>(
                name: "Investment_CurrencyId",
                table: "Deposits",
                type: "smallint",
                nullable: false);

            migrationBuilder.DropColumn(name: "Money_CurrencyId", table: "DepositOperations");
            migrationBuilder.AddColumn<byte>(
                name: "Money_CurrencyId",
                table: "DepositOperations",
                type: "smallint",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Investment_CurrencyId",
                table: "Deposits",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "Money_CurrencyId",
                table: "DepositOperations",
                type: "text",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");
        }
    }
}
