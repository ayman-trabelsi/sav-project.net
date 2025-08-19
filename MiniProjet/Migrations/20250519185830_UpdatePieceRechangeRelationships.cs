using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePieceRechangeRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PiecesRechange_Articles_ArticleId",
                table: "PiecesRechange");

            migrationBuilder.AddForeignKey(
                name: "FK_PiecesRechange_Articles_ArticleId",
                table: "PiecesRechange",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PiecesRechange_Articles_ArticleId",
                table: "PiecesRechange");

            migrationBuilder.AddForeignKey(
                name: "FK_PiecesRechange_Articles_ArticleId",
                table: "PiecesRechange",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
