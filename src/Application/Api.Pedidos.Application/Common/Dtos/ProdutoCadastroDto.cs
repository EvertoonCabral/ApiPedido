namespace Api.Pedidos.Application.Common.Dtos;

public class ProdutoCadastroDto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public string? Descricao { get; set; }
    public required decimal Preco { get; set; }
    public required decimal PrecoVenda { get; set; }
    public DateTime DataCadastro { get; set; }
}
