using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Exceptions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Api.Pedidos.Application.Clientes.Commands;

public class CadastrarClienteCommand : IRequest<int>
{
    public string Nome { get; }
    public string Email { get; }
    public string Telefone { get; }
    public bool IsAtivo { get; }
    public Endereco? Endereco { get; }

    public CadastrarClienteCommand(string nome, string email, string telefone, bool isAtivo, Endereco? endereco)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        IsAtivo = isAtivo;
        Endereco = endereco;
    }
}

public class CadastrarClienteCommandHandler : IRequestHandler<CadastrarClienteCommand, int>
{
    private readonly IClienteRepository _clienteRepo;
    private readonly IUnitOfWork _uow;
    private readonly ILogger<CadastrarClienteCommandHandler> _logger;

    public CadastrarClienteCommandHandler(
        IClienteRepository clienteRepo,
        IUnitOfWork uow,
        ILogger<CadastrarClienteCommandHandler> logger)
    {
        _clienteRepo = clienteRepo;
        _uow = uow;
        _logger = logger;
    }

    public async Task<int> Handle(CadastrarClienteCommand request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
            throw new DomainException("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new DomainException("Email é obrigatório.");

        if (string.IsNullOrWhiteSpace(request.Telefone))
            throw new DomainException("Telefone é obrigatório.");

        var email = request.Email.Trim().ToLowerInvariant();
        var telefone = request.Telefone.Trim();

        var existente = await _clienteRepo.GetByEmailAsync(email, ct);
        if (existente is not null)
            throw new DomainException("Já existe um cliente cadastrado com este email.");

        Endereco? endereco = null;
        if (request.Endereco is not null)
        {
            var e = request.Endereco;
            endereco = new Endereco(e.Rua, e.Numero, e.Bairro, e.Cidade, e.Uf, e.Cep);
        }

        var cliente = Cliente.Criar(request.Nome.Trim(), email, telefone, endereco, request.IsAtivo);

        await _clienteRepo.AddAsync(cliente, ct);
        await _uow.SaveChangesAsync(ct);

        _logger.LogInformation("Cliente {ClienteId} criado com sucesso.", cliente.Id);
        return cliente.Id;
    }
}