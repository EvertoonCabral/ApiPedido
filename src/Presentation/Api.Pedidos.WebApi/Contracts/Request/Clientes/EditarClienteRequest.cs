namespace Api.Pedidos.WebApi.Contracts.Request.Clientes;

public class EditarClienteRequest
{
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Telefone { get; set; } = default!;
    public Domain.Models.Endereco? Endereco { get; set; }
}