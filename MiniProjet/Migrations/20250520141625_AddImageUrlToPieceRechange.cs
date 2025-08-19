using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToPieceRechange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PiecesRechange",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PiecesRechange");
        }
    }
}
