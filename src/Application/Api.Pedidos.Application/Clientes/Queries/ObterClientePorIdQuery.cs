using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Queries;

public class ObterClientePorIdQuery : IRequest<Cliente>
{
    public int Id { get; set; }
    public class Handler : IRequestHandler<ObterClientePorIdQuery, Cliente>
    {
        private readonly IClienteRepository _clienteRepo;

        public Handler(IClienteRepository clienteRepo) => _clienteRepo = clienteRepo;

        public async Task<Cliente> Handle(ObterClientePorIdQuery request, CancellationToken ct)
        {
            return await _clienteRepo.GetByIdAsync(request.Id, ct)
                       ?? throw new Exception("Cliente não encontrado.");
        }
         
    }
}