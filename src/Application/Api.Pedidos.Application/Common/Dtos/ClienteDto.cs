namespace Api.Pedidos.Application.Common.Dtos;

public class ClienteDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool IsAtivo { get; set; }
    public string Telefone { get; set; } = default!;
    public int? EnderecoId { get; set; }
}