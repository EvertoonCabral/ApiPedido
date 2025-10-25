namespace Api.Pedidos.Domain.Models;

public class Produto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public string Descricao { get; set; }
    public required decimal Preco { get; set; }
    public required decimal PrecoVenda { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public bool IsAtivo { get; set; } = true;
}