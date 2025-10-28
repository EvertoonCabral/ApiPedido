using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Commands
{
    public class RemoverProdutoDoPedidoCommand : IRequest<bool>
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int QuantidadeRemover { get; set; } 

        public RemoverProdutoDoPedidoCommand(int pedidoId, int produtoId, int quantidade)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            QuantidadeRemover = quantidade;
        }
    }

    public class RemoverProdutoDoPedidoCommandHandler : IRequestHandler<RemoverProdutoDoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IUnitOfWork _uow;

        public RemoverProdutoDoPedidoCommandHandler(IPedidoRepository pedidoRepo, IUnitOfWork uow)
        {
            _pedidoRepo = pedidoRepo;
            _uow = uow;
        }

        public async Task<bool> Handle(RemoverProdutoDoPedidoCommand request, CancellationToken cancellationToken)
        {
            if (request.QuantidadeRemover <= 0) return false;

            var pedido = await _pedidoRepo.GetWithProdutosByIdAsync(request.PedidoId, cancellationToken);
            if (pedido == null) return false;

            var item = pedido.Itens.FirstOrDefault(i => i.ProdutoId == request.ProdutoId);
            if (item == null) return false;

            item.Quantidade -= request.QuantidadeRemover;

            if (item.Quantidade <= 0)
            {
                pedido.Itens.Remove(item);
            }

            pedido.DataAtualizacao = DateTime.Now;

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}