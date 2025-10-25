using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Queries;

public class ObterClientePorIdQuery : IRequest<Cliente>
{
    public int ClienteId { get; set; }

    public ObterClientePorIdQuery() { }
    public ObterClientePorIdQuery(int clienteId) => ClienteId = clienteId;

    public class Handler : IRequestHandler<ObterClientePorIdQuery, Cliente>
    {
        private readonly IClienteRepository _clienteRepo;

        public Handler(IClienteRepository clienteRepo) => _clienteRepo = clienteRepo;

        public async Task<Cliente> Handle(ObterClientePorIdQuery request, CancellationToken ct)
            => await _clienteRepo.GetByIdAsync(request.ClienteId, ct)
               ?? throw new Exception("Cliente não encontrado.");
    }
}