using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.PortfolioManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ProtoEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "OutboxMessages",
                type: "bytea",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "OutboxMessages",
                type: "jsonb",
                nullable: false);
        }
    }
}
