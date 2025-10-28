using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Produtos.Commands;

public class InativarProdutoCommand : IRequest<Unit>
{
    public int ProdutoId { get; set; }

    public InativarProdutoCommand() { }

    public InativarProdutoCommand(int produtoId)
    {
        ProdutoId = produtoId;
    }

    public class Handler(IProdutoRepository repo, IUnitOfWork uow) : IRequestHandler<InativarProdutoCommand, Unit>
    {
        public async Task<Unit> Handle(InativarProdutoCommand request, CancellationToken ct)
        {
            var produto = await repo.GetByIdAsync(request.ProdutoId, ct)
                          ?? throw new Exception("Produto não encontrado.");

            produto.Inativar();

             repo.Update(produto);
            await uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}