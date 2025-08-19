using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniProjet.Migrations
{
    /// <inheritdoc />
    public partial class AddDateAchatAndClientIdToArticle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAchat",
                table: "Articles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ClientId",
                table: "Articles",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_ClientId",
                table: "Articles",
                column: "ClientId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_ClientId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_ClientId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "DateAchat",
                table: "Articles");
        }
    }
}
