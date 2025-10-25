using Api.Pedidos.Domain.Models;

namespace Api.Pedidos.Domain.Clientes;

public class Cliente
{
 public int Id { get; set; }
 public string Nome { get; set; }
 public string Email { get; set; }
 public bool IsAtivo { get; set; }
 public string Telefone { get; set; }
 public Endereco Endereco { get; set; }
}