using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_gestionStagiaire.Migrations
{
    /// <inheritdoc />
    public partial class File : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Candidats");

            migrationBuilder.RenameColumn(
                name: "PDFPath",
                table: "Candidats",
                newName: "PdfPath");

            migrationBuilder.AlterColumn<string>(
                name: "PdfPath",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Candidats");

            migrationBuilder.RenameColumn(
                name: "PdfPath",
                table: "Candidats",
                newName: "PDFPath");

            migrationBuilder.AlterColumn<string>(
                name: "PDFPath",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Candidats",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
