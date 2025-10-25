using Api.Pedidos.WebApi.Contracts.Request.Endereco;

namespace Api.Pedidos.WebApi.Contracts.Request.Clientes;

public class CadastrarClienteRequest
{
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Telefone { get; set; } = default!;
    public bool? IsAtivo { get; set; } = true;
    public Domain.Models.Endereco? Endereco { get; set; }
}