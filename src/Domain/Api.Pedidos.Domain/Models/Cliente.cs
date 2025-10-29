namespace Api.Pedidos.Domain.Models;

public class Cliente
{
    public int Id { get; private set; }
    public string Nome { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public bool IsAtivo { get; private set; }
    public string Telefone { get; private set; } = default!;
    public Endereco? Endereco { get; private set; }

    protected Cliente()
    {
    }

    public static Cliente Criar(string nome, string email, string telefone, Endereco? endereco = null,
        bool isAtivo = true)
    {
        return new Cliente
        {
            Nome = nome,
            Email = email.Trim(),
            Telefone = telefone,
            Endereco = endereco,
            IsAtivo = isAtivo
        };
    }

    public void Inativar() => IsAtivo = false;
    
    public void Editar(string nome, string email, string telefone, Endereco? endereco)
    {
        Nome =nome;
        Email = email.Trim();
        Telefone = telefone;
        Endereco = endereco;
    }
}