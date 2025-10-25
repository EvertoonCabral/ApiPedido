using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Enums;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Commands
{
    public class CancelarPedidoCommand : IRequest<bool>
    {
        public int PedidoId { get; set; }

        public CancelarPedidoCommand(int pedidoId)
        {
            PedidoId = pedidoId;
        }
    }

    public class CancelarPedidoCommandHandler : IRequestHandler<CancelarPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IUnitOfWork _uow;

        public CancelarPedidoCommandHandler(IPedidoRepository pedidoRepo, IUnitOfWork uow)
        {
            _pedidoRepo = pedidoRepo;
            _uow = uow;
        }

        public async Task<bool> Handle(CancelarPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepo.GetWithProdutosByIdAsync(request.PedidoId, cancellationToken);
            if (pedido == null) return false;


            if (pedido.Status != StatusPedido.ABERTO)
                return false;

            pedido.Status = StatusPedido.CANCELADO;
            pedido.DataAtualizacao = System.DateTime.UtcNow;

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}