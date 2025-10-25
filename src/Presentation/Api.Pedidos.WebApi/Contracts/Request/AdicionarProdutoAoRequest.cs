namespace Api.Pedidos.WebApi.Contracts.Request;

public class AdicionarProdutoAoPedidoRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }
}