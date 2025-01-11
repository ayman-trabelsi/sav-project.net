using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class interfdcscs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations");

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations",
                column: "InterventionId");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions",
                column: "ReclamationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions",
                column: "ReclamationId",
                principalTable: "Reclamations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions");

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations",
                column: "InterventionId",
                unique: true,
                filter: "[InterventionId] IS NOT NULL");
        }
    }
}
