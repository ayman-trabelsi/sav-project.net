using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class intervention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Articles_ArticleId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ArticleId",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "EstGratuite",
                table: "Interventions");

            migrationBuilder.RenameColumn(
                name: "ArticleId",
                table: "Interventions",
                newName: "ReclamationId");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontantFacture",
                table: "Interventions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations",
                column: "InterventionId",
                unique: true,
                filter: "[InterventionId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations");

            migrationBuilder.RenameColumn(
                name: "ReclamationId",
                table: "Interventions",
                newName: "ArticleId");

            migrationBuilder.AlterColumn<decimal>(
                name: "MontantFacture",
                table: "Interventions",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<bool>(
                name: "EstGratuite",
                table: "Interventions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations",
                column: "InterventionId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ArticleId",
                table: "Interventions",
                column: "ArticleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Articles_ArticleId",
                table: "Interventions",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
