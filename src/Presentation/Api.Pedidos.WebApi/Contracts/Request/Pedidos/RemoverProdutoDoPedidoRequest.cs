namespace Api.Pedidos.WebApi.Contracts.Request;

public class RemoverProdutoDoPedidoRequest
{
    public int ProdutoId { get; set; }
    public int Quantidade { get; set; }    
}