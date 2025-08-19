using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Update user types based on role
            migrationBuilder.Sql("UPDATE Users SET UserType = 'Client' WHERE Role = 'Client'");
            migrationBuilder.Sql("UPDATE Users SET UserType = 'ResponsableSAV' WHERE Role = 'ResponsableSAV'");
            migrationBuilder.Sql("UPDATE Users SET UserType = 'Technicien' WHERE Role = 'Technicien'");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert user types back to 'User'
            migrationBuilder.Sql("UPDATE Users SET UserType = 'User'");
        }
    }
}
