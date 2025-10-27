namespace Api.Pedidos.Domain.Models;

public class Produto
{
    public int Id { get; set; }
    public required string Nome { get; set; }
    public required string Descricao { get; set; }
    public required decimal Preco { get; set; }
    public required decimal PrecoVenda { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public bool IsAtivo { get; set; } = true;


    public static Produto Criar(string nome, string descricao, decimal preco, decimal precoVenda,
        DateTime dataCadastro, DateTime dataAtualizacao, bool isAtivo)
    {
        return new Produto
        {
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            PrecoVenda = precoVenda,
            DataCadastro = dataCadastro,
            DataAtualizacao = dataAtualizacao,
            IsAtivo = isAtivo
        };
    }
}