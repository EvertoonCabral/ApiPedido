using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Api.Pedidos.Application.Produtos.Queries;

public class ListarProdutosQuery : IRequest<IReadOnlyList<Produto>>
{
    public class Handler : IRequestHandler<ListarProdutosQuery, IReadOnlyList<Produto>>
    {
        private readonly IProdutoRepository _repo;

        public Handler(IProdutoRepository repo, IMapper mapper)
        {
            _repo = repo;
        }

        public async Task<IReadOnlyList<Produto>> Handle(ListarProdutosQuery request, CancellationToken ct)
        {
            var produtos = await _repo.ListAsync(ct);
            return produtos;
        }
    }
}