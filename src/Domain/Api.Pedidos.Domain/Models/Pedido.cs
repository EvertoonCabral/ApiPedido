using Api.Pedidos.Domain.Enums;

namespace Api.Pedidos.Domain.Models;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public DateTime DataAbertura { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public StatusPedido Status { get; set; }
    public List<Produto> Produtos { get; set; }
}