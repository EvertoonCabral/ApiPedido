namespace Api.Pedidos.Application.Common.Dtos;

public record ItemPedidoDto(int Id, int ProdutoId, string NomeProduto, decimal PrecoUnitario, int Quantidade);

public record PedidoDto(
    int Id,
    int ClienteId,
    DateTime DataAbertura,
    DateTime DataAtualizacao,
    string Status,
    decimal Total,
    List<ItemPedidoDto> Itens
);