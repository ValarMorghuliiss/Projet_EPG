using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_gestionStagiaire.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationSujetDeStageStagiaire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SujetDeStageId",
                table: "Stagiaires",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stagiaires_SujetDeStageId",
                table: "Stagiaires",
                column: "SujetDeStageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stagiaires_SujetsDeStage_SujetDeStageId",
                table: "Stagiaires",
                column: "SujetDeStageId",
                principalTable: "SujetsDeStage",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stagiaires_SujetsDeStage_SujetDeStageId",
                table: "Stagiaires");

            migrationBuilder.DropIndex(
                name: "IX_Stagiaires_SujetDeStageId",
                table: "Stagiaires");

            migrationBuilder.DropColumn(
                name: "SujetDeStageId",
                table: "Stagiaires");
        }
    }
}
