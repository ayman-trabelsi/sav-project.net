using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class FixInterventionReclamationRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reclamations_Interventions_InterventionId",
                table: "Reclamations");

            migrationBuilder.DropIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations");

            migrationBuilder.CreateIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions",
                column: "ReclamationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions",
                column: "ReclamationId",
                principalTable: "Reclamations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Reclamations_ReclamationId",
                table: "Interventions");

            migrationBuilder.DropIndex(
                name: "IX_Interventions_ReclamationId",
                table: "Interventions");

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_InterventionId",
                table: "Reclamations",
                column: "InterventionId",
                unique: true,
                filter: "[InterventionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Reclamations_Interventions_InterventionId",
                table: "Reclamations",
                column: "InterventionId",
                principalTable: "Interventions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
