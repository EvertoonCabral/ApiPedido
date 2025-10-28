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

    public static Produto Criar(string nome, string descricao, decimal preco, decimal precoVenda, bool isAtivo = true)
    {
        return new Produto
        {
            Nome = nome,
            Descricao = descricao,
            Preco = preco,
            PrecoVenda = precoVenda,
            DataCadastro = DateTime.Now,
            DataAtualizacao = DateTime.Now,
            IsAtivo = isAtivo
        };
    }

    public void Editar(string nome, string descricao, decimal preco, decimal precoVenda)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        PrecoVenda = precoVenda;
        DataAtualizacao = DateTime.Now;
    }

    public void Inativar()
    {
        IsAtivo = false;
        DataAtualizacao = DateTime.Now;
    }
    
    public void Ativar()
    {
        IsAtivo = true;
        DataAtualizacao = DateTime.Now;
    }
}