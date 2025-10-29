using Xunit;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Enums; 
using FluentAssertions;
namespace Api.Pedidos.Domain.Tests.Models;

public class PedidoTests
{
    [Fact]
    public void Total_DeveCalcularSomaDosItens()
    {
        // Arrange
        var pedido = new Pedido
        {
            ClienteId = 1,
            Status = StatusPedido.Aberto,
            Itens = new List<ItemPedido>
            {
                new() { ProdutoId = 1, Quantidade = 2, PrecoUnitario = 100m },
                new() { ProdutoId = 2, Quantidade = 3, PrecoUnitario = 50m }
            }
        };

        // Act
        var total = pedido.Total;

        // Assert
        total.Should().Be(350m); // (2 * 100) + (3 * 50)
    }

    [Fact]
    public void Total_ComListaVazia_DeveRetornarZero()
    {
        // Arrange
        var pedido = new Pedido
        {
            ClienteId = 1,
            Itens = new List<ItemPedido>()
        };

        // Act
        var total = pedido.Total;

        // Assert
        total.Should().Be(0m);
    }
}