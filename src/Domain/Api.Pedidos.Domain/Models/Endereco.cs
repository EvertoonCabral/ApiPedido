namespace Api.Pedidos.Domain.Models;

public class Endereco
{

    public string Rua { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string Cep { get; set; } = null!;

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