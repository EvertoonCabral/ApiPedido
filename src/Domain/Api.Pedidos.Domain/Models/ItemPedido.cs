namespace Api.Pedidos.Domain.Models;

public class ItemPedido
{
    public int Id { get; set; }

    public int PedidoId { get; set; }

    public int ProdutoId { get; set; }

    public string NomeProduto { get; set; } = null!;

    public decimal PrecoUnitario { get; set; }

    public int Quantidade { get; set; }

    public Produto? Produto { get; set; }
}