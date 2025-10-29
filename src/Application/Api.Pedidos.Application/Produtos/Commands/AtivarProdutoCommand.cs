using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Produtos.Commands;

public class AtivarProdutoCommand : IRequest<Unit>
{
    public int ProdutoId { get; set; }

    public AtivarProdutoCommand() { }
    public AtivarProdutoCommand(int produtoId) => ProdutoId = produtoId;

    public class Handler : IRequestHandler<AtivarProdutoCommand, Unit>
    {
        private readonly IProdutoRepository _repo;
        private readonly IUnitOfWork _uow;

        public Handler(IProdutoRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<Unit> Handle(AtivarProdutoCommand request, CancellationToken ct)
        {
            var produto = await _repo.GetByIdAsync(request.ProdutoId, ct)
                          ?? throw new Exception("Produto não encontrado.");

            produto.Ativar();

            await _repo.UpdateAsync(produto, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}