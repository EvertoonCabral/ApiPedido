using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Pedidos.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Ajuste_Tamanho_Estado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco_Estado",
                table: "Clientes",
                newName: "Endereco_Uf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco_Uf",
                table: "Clientes",
                newName: "Endereco_Estado");
        }
    }
}
