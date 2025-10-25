namespace Api.Pedidos.Domain.Models;

public class Endereco
{

    public string Rua { get; private set; } = null!;
    public string Numero { get; private set; } = null!;
    public string Bairro { get; private set; } = null!;
    public string Cidade { get; private set; } = null!;
    public string Estado { get; private set; } = null!;
    public string Cep { get; private set; } = null!;

    protected Endereco() { }

    public Endereco(string rua, string numero, string bairro, string cidade, string estado, string cep)
    {
        Rua = rua;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        Cep = cep;
    }
}