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
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            if (pedido.Status == StatusPedido.Cancelado)
                throw new InvalidOperationException("Pedido já está cancelado.");

            if (pedido.Status == StatusPedido.Fechado)
                throw new InvalidOperationException("Não é possível cancelar um pedido já fechado.");

            pedido.Status = StatusPedido.Cancelado;
            pedido.DataAtualizacao = DateTime.UtcNow;

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}