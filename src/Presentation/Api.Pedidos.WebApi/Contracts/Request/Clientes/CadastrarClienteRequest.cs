using System.ComponentModel.DataAnnotations;
namespace Api.Pedidos.WebApi.Contracts.Request.Clientes;

public class CadastrarClienteRequest
{
    [Required]
    public string Nome { get; set; } = default!;
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Telefone { get; set; } = default!;
    public bool? IsAtivo { get; set; } = true;
    public Domain.Models.Endereco? Endereco { get; set; }
}