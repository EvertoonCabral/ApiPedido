using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Enums;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Pedidos.Application.Pedidos.Queries
{
    public class ListarPedidosQuery : IRequest<PageResponse<PedidoDto>>
    {
        public int? ClienteId { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public ListarPedidosQuery() { }

        public ListarPedidosQuery(int? clienteId, string? status, int page, int pageSize)
        {
            ClienteId = clienteId;
            Status = status;
            Page = page;
            PageSize = pageSize;
        }
    }

    public class ListarPedidosQueryHandler : IRequestHandler<ListarPedidosQuery, PageResponse<PedidoDto>>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IMapper _mapper;

        public ListarPedidosQueryHandler(IPedidoRepository pedidoRepo, IMapper mapper)
        {
            _pedidoRepo = pedidoRepo;
            _mapper = mapper;
        }

        public async Task<PageResponse<PedidoDto>> Handle(ListarPedidosQuery request, CancellationToken cancellationToken)
        {
            var page = request.Page <= 0 ? 1 : request.Page;
            var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;
            if (pageSize > 100) pageSize = 100;

            var queryable = _pedidoRepo.Query();

            if (request.ClienteId.HasValue)
                queryable = queryable.Where(p => p.ClienteId == request.ClienteId.Value);

            if (!string.IsNullOrWhiteSpace(request.Status) &&
                Enum.TryParse<StatusPedido>(request.Status, ignoreCase: true, out var status))
            {
                queryable = queryable.Where(p => p.Status == status);
            }

            queryable = queryable.OrderByDescending(p => p.DataAbertura);

            var totalItems = await queryable.LongCountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pedidos = await queryable
                .Include(p => p.Itens)
                .AsNoTracking()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var items = _mapper.Map<List<PedidoDto>>(pedidos);

            return new PageResponse<PedidoDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
        }
    }
}