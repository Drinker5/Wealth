using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.StrategyTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategyComponent_Strategies_StrategyId",
                table: "StrategyComponent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StrategyComponent",
                table: "StrategyComponent");

            migrationBuilder.RenameTable(
                name: "StrategyComponent",
                newName: "StrategyComponents");

            migrationBuilder.RenameColumn(
                name: "InstrumentId",
                table: "StrategyComponents",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "BondId",
                table: "StrategyComponents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyId",
                table: "StrategyComponents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "StrategyComponents",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "type",
                table: "StrategyComponents",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StrategyComponents",
                table: "StrategyComponents",
                columns: new[] { "StrategyId", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyComponents_Strategies_StrategyId",
                table: "StrategyComponents",
                column: "StrategyId",
                principalTable: "Strategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategyComponents_Strategies_StrategyId",
                table: "StrategyComponents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StrategyComponents",
                table: "StrategyComponents");

            migrationBuilder.DropColumn(
                name: "BondId",
                table: "StrategyComponents");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "StrategyComponents");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "StrategyComponents");

            migrationBuilder.DropColumn(
                name: "type",
                table: "StrategyComponents");

            migrationBuilder.RenameTable(
                name: "StrategyComponents",
                newName: "StrategyComponent");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StrategyComponent",
                newName: "InstrumentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StrategyComponent",
                table: "StrategyComponent",
                columns: new[] { "StrategyId", "InstrumentId" });

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyComponent_Strategies_StrategyId",
                table: "StrategyComponent",
                column: "StrategyId",
                principalTable: "Strategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
