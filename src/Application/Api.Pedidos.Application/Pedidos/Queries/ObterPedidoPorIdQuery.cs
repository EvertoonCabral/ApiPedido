using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Queries
{
    public class ObterPedidoPorIdQuery : IRequest<PedidoDto?>
    {
        public int Id { get; }

        public ObterPedidoPorIdQuery(int id)
        {
            Id = id;
        }
    }

    public class ObterPedidoPorIdQueryHandler : IRequestHandler<ObterPedidoPorIdQuery, PedidoDto?>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IMapper _mapper;

        public ObterPedidoPorIdQueryHandler(IPedidoRepository pedidoRepo, IMapper mapper)
        {
            _pedidoRepo = pedidoRepo;
            _mapper = mapper;
        }

        public async Task<PedidoDto?> Handle(ObterPedidoPorIdQuery request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepo.GetWithProdutosByIdAsync(request.Id, cancellationToken);
            if (pedido == null) return null;

            return _mapper.Map<PedidoDto>(pedido);
        }
    }
}