using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wealth.CurrencyManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OutboxRefactor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Error",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "ProcessedDate",
                table: "OutboxMessages");

            migrationBuilder.RenameColumn(
                name: "ProcessingDate",
                table: "OutboxMessages",
                newName: "ProcessedOn");

            migrationBuilder.RenameColumn(
                name: "AssemblyName",
                table: "OutboxMessages",
                newName: "Key");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "OutboxMessages",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.DropColumn(
                name: "Data",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<byte[]>(
                name: "Data",
                table: "OutboxMessages",
                type: "bytea",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "DefferedCommands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OccurredOn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    AssemblyName = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    ProcessedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ProcessingDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DefferedCommands", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DefferedCommands");

            migrationBuilder.RenameColumn(
                name: "ProcessedOn",
                table: "OutboxMessages",
                newName: "ProcessingDate");

            migrationBuilder.RenameColumn(
                name: "Key",
                table: "OutboxMessages",
                newName: "AssemblyName");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "OutboxMessages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.DropColumn(
                name: "Data",
                table: "OutboxMessages");

            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "OutboxMessages",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Error",
                table: "OutboxMessages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ProcessedDate",
                table: "OutboxMessages",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
