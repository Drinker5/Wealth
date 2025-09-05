using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.StrategyTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Refactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StrategyComponents_Strategies_StrategyId",
                table: "StrategyComponents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StrategyComponents",
                table: "StrategyComponents");

            migrationBuilder.RenameTable(
                name: "StrategyComponents",
                newName: "StrategyComponent");

            migrationBuilder.DropColumn(name: "CurrencyId", table: "StrategyComponent");
            migrationBuilder.AddColumn<byte>(
                name: "CurrencyId",
                table: "StrategyComponent",
                type: "smallint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "StrategyComponent",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StrategyComponent",
                table: "StrategyComponent",
                columns: new[] { "StrategyId", "type", "Id" });

            migrationBuilder.AddForeignKey(
                name: "FK_StrategyComponent_Strategies_StrategyId",
                table: "StrategyComponent",
                column: "StrategyId",
                principalTable: "Strategies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyId",
                table: "StrategyComponents",
                type: "text",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "StrategyComponents",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

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
    }
}
