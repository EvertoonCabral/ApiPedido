using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Enums;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Commands
{
    public class FecharPedidoCommand : IRequest<bool>
    {
        public int PedidoId { get; set; }

        public FecharPedidoCommand(int pedidoId)
        {
            PedidoId = pedidoId;
        }
    }

    public class FecharPedidoCommandHandler : IRequestHandler<FecharPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IUnitOfWork _uow;

        public FecharPedidoCommandHandler(IPedidoRepository pedidoRepo, IUnitOfWork uow)
        {
            _pedidoRepo = pedidoRepo;
            _uow = uow;
        }

        public async Task<bool> Handle(FecharPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepo.GetWithProdutosByIdAsync(request.PedidoId, cancellationToken);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            if (pedido.Itens == null || !pedido.Itens.Any())
                throw new InvalidOperationException("Pedido sem itens não pode ser fechado.");

            if (pedido.Status != StatusPedido.Aberto)
                throw new InvalidOperationException("Apenas pedidos em status 'Aberto' podem ser fechados.");

            pedido.Status = StatusPedido.Fechado;
            pedido.DataAtualizacao = DateTime.UtcNow;

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}