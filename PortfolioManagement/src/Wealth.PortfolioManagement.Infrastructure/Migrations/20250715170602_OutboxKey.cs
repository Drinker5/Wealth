using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OutboxKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "OutboxMessages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "OutboxMessages");
        }
    }
}
