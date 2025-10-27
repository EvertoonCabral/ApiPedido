using System.ComponentModel.DataAnnotations;

namespace Api.Pedidos.WebApi.Contracts.Request.Produtos;

public class CadastrarProdutoRequest
{
    [Required]
    public required string Nome { get; set; }
    [Required]
    public required string Descricao { get; set; }
    [Required]
    public required decimal Preco { get; set; }
    [Required]
    public required decimal PrecoVenda { get; set; }
}