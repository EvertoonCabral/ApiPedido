using Api.Pedidos.Domain.Models;
using FluentAssertions;
using Xunit;

namespace Api.Pedidos.Domain.Tests.Models;

public class ProdutoTests
{
    [Fact]
    public void Criar_DeveRetornarProdutoValido()
    {
        // Arrange
        var nome = "Notebook";
        var descricao = "Notebook Dell";
        var preco = 2000m;
        var precoVenda = 2500m;

        // Act
        var produto = Produto.Criar(nome, descricao, preco, precoVenda);

        // Assert
        produto.Should().NotBeNull();
        produto.Nome.Should().Be(nome);
        produto.Descricao.Should().Be(descricao);
        produto.Preco.Should().Be(preco);
        produto.PrecoVenda.Should().Be(precoVenda);
        produto.IsAtivo.Should().BeTrue();
        produto.DataCadastro.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Editar_DeveAtualizarDadosDoProduto()
    {
        // Arrange
        var produto = Produto.Criar("Notebook", "Dell", 2000m, 2500m);
        var dataAntes = produto.DataAtualizacao;
        Thread.Sleep(10); // Garante diferença de tempo

        // Act
        produto.Editar("Notebook Gamer", "Dell G15", 3000m, 3500m);

        // Assert
        produto.Nome.Should().Be("Notebook Gamer");
        produto.Descricao.Should().Be("Dell G15");
        produto.Preco.Should().Be(3000m);
        produto.PrecoVenda.Should().Be(3500m);
        produto.DataAtualizacao.Should().BeAfter(dataAntes);
    }

    [Fact]
    public void Inativar_DeveAlterarStatusParaInativo()
    {
        // Arrange
        var produto = Produto.Criar("Notebook", "Dell", 2000m, 2500m);

        // Act
        produto.Inativar();

        // Assert
        produto.IsAtivo.Should().BeFalse();
    }

    [Fact]
    public void Ativar_DeveAlterarStatusParaAtivo()
    {
        // Arrange
        var produto = Produto.Criar("Notebook", "Dell", 2000m, 2500m, isAtivo: false);

        // Act
        produto.Ativar();

        // Assert
        produto.IsAtivo.Should().BeTrue();
    }
}