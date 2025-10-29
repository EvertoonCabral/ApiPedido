using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Api.Pedidos.Application.Produtos.Queries
{
    public class ObterProdutoPorIdQuery : IRequest<Produto?>
    {
        public int ProdutoId { get; set; }
        
    }

    public class ObterProdutoPorIdQueryHandler : IRequestHandler<ObterProdutoPorIdQuery, Produto?>
    {
        private readonly IProdutoRepository _produtoRepo;
        private readonly IMapper _mapper;

        public ObterProdutoPorIdQueryHandler(IProdutoRepository produtoRepo, IMapper mapper)
        {
            _produtoRepo = produtoRepo;
            _mapper = mapper;
        }

        public async Task<Produto?> Handle(ObterProdutoPorIdQuery request, CancellationToken cancellationToken)
        {
            var produto = await _produtoRepo.GetByIdAsync(request.ProdutoId, cancellationToken);
            if (produto == null) return null;

            return produto;
        }
    }
}