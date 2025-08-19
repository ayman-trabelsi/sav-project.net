using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterventionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Techniciens_TechnicienId",
                table: "Interventions");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Techniciens_TechnicienId",
                table: "Interventions",
                column: "TechnicienId",
                principalTable: "Techniciens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Interventions_Techniciens_TechnicienId",
                table: "Interventions");

            migrationBuilder.AddForeignKey(
                name: "FK_Interventions_Techniciens_TechnicienId",
                table: "Interventions",
                column: "TechnicienId",
                principalTable: "Techniciens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
