using Xunit;
using Api.Pedidos.Domain.Models;
using FluentAssertions;

namespace Api.Pedidos.Domain.Tests.Models;

public class ClienteTests
{
    [Fact]
    public void Criar_DeveRetornarClienteValido()
    {
        // Arrange
        var nome = "João Silva";
        var email = "joao@email.com";
        var telefone = "11999999999";

        // Act
        var cliente = Cliente.Criar(nome, email, telefone);

        // Assert
        cliente.Should().NotBeNull();
        cliente.Nome.Should().Be(nome);
        cliente.Email.Should().Be(email);
        cliente.Telefone.Should().Be(telefone);
        cliente.IsAtivo.Should().BeTrue();
    }

    [Fact]
    public void Criar_DeveTrimmarEmail()
    {
        // Arrange & Act
        var cliente = Cliente.Criar("João", "  joao@email.com  ", "11999999999");

        // Assert
        cliente.Email.Should().Be("joao@email.com");
    }

    [Fact]
    public void Criar_ComEnderecoNulo_DevePermitir()
    {
        // Arrange & Act
        var cliente = Cliente.Criar("João", "joao@email.com", "11999999999", endereco: null);

        // Assert
        cliente.Endereco.Should().BeNull();
    }

    [Fact]
    public void Inativar_DeveAlterarStatusParaInativo()
    {
        // Arrange
        var cliente = Cliente.Criar("João", "joao@email.com", "11999999999");

        // Act
        cliente.Inativar();

        // Assert
        cliente.IsAtivo.Should().BeFalse();
    }

    [Fact]
    public void Editar_DeveAtualizarDadosDoCliente()
    {
        // Arrange
        var cliente = Cliente.Criar("João", "joao@email.com", "11999999999");
        var novoNome = "João Pedro Silva";
        var novoEmail = "joaopedro@email.com";
        var novoTelefone = "11988888888";

        // Act
        cliente.Editar(novoNome, novoEmail, novoTelefone, null);

        // Assert
        cliente.Nome.Should().Be(novoNome);
        cliente.Email.Should().Be(novoEmail);
        cliente.Telefone.Should().Be(novoTelefone);
    }
}