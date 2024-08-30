using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_gestionStagiaire.Migrations
{
    /// <inheritdoc />
    public partial class CandidatV1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvPath",
                table: "Candidats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CvPath",
                table: "Candidats",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }
    }
}
