using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Enums;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Commands
{
    public class IniciarPedidoCommand : IRequest<int>
    {
        public int ClienteId { get; set; }

        public IniciarPedidoCommand(int clienteId)
        {
            ClienteId = clienteId;
        }
    }

    public class IniciarPedidoCommandHandler : IRequestHandler<IniciarPedidoCommand, int>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IUnitOfWork _uow;

        public IniciarPedidoCommandHandler(IPedidoRepository pedidoRepo, IUnitOfWork uow)
        {
            _pedidoRepo = pedidoRepo;
            _uow = uow;
        }

        public async Task<int> Handle(IniciarPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = new Pedido
            {
                ClienteId = request.ClienteId,
                DataAbertura = DateTime.UtcNow,
                DataAtualizacao = DateTime.UtcNow,
                Status = StatusPedido.Aberto,
                Itens = new()
            };

            await _pedidoRepo.AddAsync(pedido, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return pedido.Id;
        }
    }
}