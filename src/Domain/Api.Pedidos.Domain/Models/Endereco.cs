using System.ComponentModel.DataAnnotations;

namespace Api.Pedidos.Domain.Models;

public class Endereco
{

    public string? Rua { get; set; }
    public string? Numero { get; set; }
    public string? Bairro { get; set; } 
    public string? Cidade { get; set; }
    
    [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado (UF) deve ter 2 caracteres.")]
    public string? Uf { get; set; } 
    public string? Cep { get; set; }
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