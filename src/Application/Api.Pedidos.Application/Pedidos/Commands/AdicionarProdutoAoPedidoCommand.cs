using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Commands
{
    public class AdicionarProdutoAoPedidoCommand : IRequest<bool>
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }

        public AdicionarProdutoAoPedidoCommand(int pedidoId, int produtoId, int quantidade)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }
    }

    public class AdicionarProdutoAoPedidoCommandHandler : IRequestHandler<AdicionarProdutoAoPedidoCommand, bool>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IProdutoRepository _produtoRepo;
        private readonly IUnitOfWork _uow;

        public AdicionarProdutoAoPedidoCommandHandler(
            IPedidoRepository pedidoRepo,
            IProdutoRepository produtoRepo,
            IUnitOfWork uow)
        {
            _pedidoRepo = pedidoRepo;
            _produtoRepo = produtoRepo;
            _uow = uow;
        }

        public async Task<bool> Handle(AdicionarProdutoAoPedidoCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepo.GetByIdAsync(request.PedidoId, cancellationToken);
            if (pedido == null) return false;

            var produto = await _produtoRepo.GetByIdAsync(request.ProdutoId, cancellationToken);
            if (produto == null || !produto.IsAtivo) throw new Exception("Produto deve estar ativo");

            var item = pedido.Itens.FirstOrDefault(i => i.ProdutoId == request.ProdutoId);
            if (item == null)
            {
                pedido.Itens.Add(new ItemPedido
                {
                    ProdutoId = produto.Id,
                    NomeProduto = produto.Nome,
                    PrecoUnitario = produto.PrecoVenda,
                    Quantidade = request.Quantidade,
                    PedidoId = pedido.Id
                });
            }
            else
            {
                item.Quantidade += request.Quantidade;
            }

            pedido.DataAtualizacao = DateTime.Now;

            await _uow.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}