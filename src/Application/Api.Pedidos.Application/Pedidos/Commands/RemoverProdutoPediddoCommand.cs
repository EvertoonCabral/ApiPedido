using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Enums;
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
            if (request.QuantidadeRemover <= 0)
                throw new ArgumentOutOfRangeException(nameof(request.QuantidadeRemover), "Quantidade a remover deve ser maior que zero.");

            var pedido = await _pedidoRepo.GetWithItemAndProdutoAsync(request.PedidoId, request.ProdutoId, cancellationToken);
            if (pedido == null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            if (pedido.Status != StatusPedido.Aberto)
                throw new InvalidOperationException("Não é possível remover produtos de pedidos fechados.");

            var item = pedido.Itens.FirstOrDefault(i => i.ProdutoId == request.ProdutoId);
            if (item == null)
                throw new KeyNotFoundException("Item do pedido não encontrado para o produto informado.");

            item.Quantidade -= request.QuantidadeRemover;

            if (item.Quantidade <= 0)
            {
                pedido.Itens.Remove(item);
            }

            pedido.DataAtualizacao = DateTime.UtcNow;

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}