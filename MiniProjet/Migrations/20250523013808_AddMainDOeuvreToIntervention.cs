using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class AddMainDOeuvreToIntervention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PiecesRechange");

            migrationBuilder.AddColumn<decimal>(
                name: "MainDOeuvre",
                table: "Interventions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainDOeuvre",
                table: "Interventions");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PiecesRechange",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
