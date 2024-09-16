using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_gestionStagiaire.Migrations
{
    /// <inheritdoc />
    public partial class FileVStagiare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Stagiaires",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PdfPath",
                table: "Stagiaires",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "PdfPath",
                table: "Stagiaires");
        }
    }
}
