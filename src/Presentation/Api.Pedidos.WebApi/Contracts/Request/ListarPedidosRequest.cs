namespace Api.Pedidos.WebApi.Contracts.Request;

public class ListarPedidosRequest
{
    public int? ClienteId { get; set; }
    public string? Status { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}