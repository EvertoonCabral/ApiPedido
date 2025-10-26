using System.ComponentModel.DataAnnotations;

namespace Api.Pedidos.Domain.Models;

public class Endereco
{

    public string Rua { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado (UF) deve ter 2 caracteres.")]
    public string Uf { get; set; } = null!;
    public string Cep { get; set; } = null!;

    protected Endereco() { }

    public Endereco(string rua, string numero, string bairro, string cidade, string uf, string cep)
    {
        Rua = rua;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Uf = uf;
        Cep = cep;
    }
}