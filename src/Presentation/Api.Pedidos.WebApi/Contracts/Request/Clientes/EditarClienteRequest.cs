using System.ComponentModel.DataAnnotations;

namespace Api.Pedidos.WebApi.Contracts.Request.Clientes;

public class EditarClienteRequest
{
    [Required]
    public string Nome { get; set; } = default!;
    [Required, EmailAddress]
    public string Email { get; set; } = default!;
    [Required]
    public string Telefone { get; set; } = default!;
    public Domain.Models.Endereco? Endereco { get; set; }
}