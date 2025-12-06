using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.StrategyTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CurrencyComponent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("Id", "StrategyComponent");
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "StrategyComponent",
                nullable: false);

            migrationBuilder.AddColumn<byte>(
                name: "CurrencyId",
                table: "StrategyComponent",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "StrategyComponent");

            migrationBuilder.DropColumn("Id", "StrategyComponent");
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "StrategyComponent",
                nullable: false);
        }
    }
}