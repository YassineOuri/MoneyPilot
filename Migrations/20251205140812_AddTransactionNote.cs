using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyPilot.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "dateTime",
                table: "Transaction",
                newName: "DateTime");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Transaction",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "Transaction",
                newName: "dateTime");
        }
    }
}
