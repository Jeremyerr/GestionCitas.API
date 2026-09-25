using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionCitas.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNumeroJVPM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroColegiado",
                table: "Medicos",
                newName: "NumeroJVPM");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumeroJVPM",
                table: "Medicos",
                newName: "NumeroColegiado");
        }
    }
}
