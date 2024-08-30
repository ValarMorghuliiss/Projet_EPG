using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_gestionStagiaire.Migrations
{
    /// <inheritdoc />
    public partial class EncadrantStagiare : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Admins",
                newName: "Username");

            migrationBuilder.CreateTable(
                name: "Encadrants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encadrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stagiaires",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusStage = table.Column<bool>(type: "bit", nullable: false),
                    Ville = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Universite = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnneeUniversitaire = table.Column<int>(type: "int", nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MotDePasse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DureDeStage = table.Column<int>(type: "int", nullable: false),
                    EncadrantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stagiaires", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stagiaires_Encadrants_EncadrantId",
                        column: x => x.EncadrantId,
                        principalTable: "Encadrants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_EncadrantId",
                table: "Stagiaires",
                column: "EncadrantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stagiaires");

            migrationBuilder.DropTable(
                name: "Encadrants");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Admins",
                newName: "Email");
        }
    }
}
