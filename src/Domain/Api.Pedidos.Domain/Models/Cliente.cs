using Api.Pedidos.Domain.Models;

public class Cliente
{

    public int Id { get; private set; }
    public string Nome { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public bool IsAtivo { get; private set; }
    public string Telefone { get; private set; } = null!;
    public Endereco Endereco { get; private set; } = null!;

    protected Cliente() { }
    
    public static Cliente Criar(string nome, string email, string telefone, Endereco endereco, bool isAtivo = true)
    {
        return new Cliente
        {
            Nome = nome,
            Email = email,
            Telefone = telefone,
            Endereco = endereco,
            IsAtivo = isAtivo
        };
    }
}