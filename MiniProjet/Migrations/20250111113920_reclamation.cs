using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class reclamation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EtatId",
                table: "Reclamations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "idArticleReclamation",
                table: "Reclamations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Etats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etats", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Etats",
                columns: new[] { "Id", "Libelle" },
                values: new object[,]
                {
                    { 1, "En Attente" },
                    { 2, "Traité" },
                    { 3, "En Cours" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_EtatId",
                table: "Reclamations",
                column: "EtatId");

            migrationBuilder.CreateIndex(
                name: "IX_Reclamations_idArticleReclamation",
                table: "Reclamations",
                column: "idArticleReclamation");

            migrationBuilder.AddForeignKey(
                name: "FK_Reclamations_Articles_idArticleReclamation",
                table: "Reclamations",
                column: "idArticleReclamation",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reclamations_Etats_EtatId",
                table: "Reclamations",
                column: "EtatId",
                principalTable: "Etats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reclamations_Articles_idArticleReclamation",
                table: "Reclamations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reclamations_Etats_EtatId",
                table: "Reclamations");

            migrationBuilder.DropTable(
                name: "Etats");

            migrationBuilder.DropIndex(
                name: "IX_Reclamations_EtatId",
                table: "Reclamations");

            migrationBuilder.DropIndex(
                name: "IX_Reclamations_idArticleReclamation",
                table: "Reclamations");

            migrationBuilder.DropColumn(
                name: "EtatId",
                table: "Reclamations");

            migrationBuilder.DropColumn(
                name: "idArticleReclamation",
                table: "Reclamations");
        }
    }
}
