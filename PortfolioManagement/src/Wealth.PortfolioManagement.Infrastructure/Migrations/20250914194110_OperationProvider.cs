using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OperationProvider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Quantity",
                table: "InstrumentOperations",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "InstrumentOperations",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount_Amount",
                table: "InstrumentOperations",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Amount_CurrencyId",
                table: "InstrumentOperations",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PortfolioIdMaps",
                columns: table => new
                {
                    AccountId = table.Column<string>(type: "text", nullable: false),
                    PortfolioId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioIdMaps", x => x.AccountId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PortfolioIdMaps");

            migrationBuilder.DropColumn(
                name: "Amount_Amount",
                table: "InstrumentOperations");

            migrationBuilder.DropColumn(
                name: "Amount_CurrencyId",
                table: "InstrumentOperations");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "InstrumentOperations",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "InstrumentOperations",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
