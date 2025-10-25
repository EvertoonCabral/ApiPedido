using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Pedidos.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AjustesModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Endereco_Id",
                table: "Clientes");

            migrationBuilder.RenameColumn(
                name: "Endereco_Logradouro",
                table: "Clientes",
                newName: "Endereco_Rua");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Endereco_Rua",
                table: "Clientes",
                newName: "Endereco_Logradouro");

            migrationBuilder.AddColumn<int>(
                name: "Endereco_Id",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
