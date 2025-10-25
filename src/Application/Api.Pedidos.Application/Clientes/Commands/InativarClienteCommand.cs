using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Commands;

public class InativarClienteCommand : IRequest<Unit>
{
    public int ClienteId { get; set; }

    public InativarClienteCommand() { }
    public InativarClienteCommand(int clienteId) => ClienteId = clienteId;

    public class Handler : IRequestHandler<InativarClienteCommand, Unit>
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public Handler(IClienteRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<Unit> Handle(InativarClienteCommand request, CancellationToken ct)
        {
            var cliente = await _repo.GetByIdAsync(request.ClienteId, ct)
                          ?? throw new Exception("Cliente não encontrado.");

            cliente.Inativar();

            await _repo.UpdateAsync(cliente);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}