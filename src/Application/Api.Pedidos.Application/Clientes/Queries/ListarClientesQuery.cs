using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Queries;

public class ListarClientesQuery : IRequest<IReadOnlyList<ClienteDto>>
{
    public class Handler : IRequestHandler<ListarClientesQuery, IReadOnlyList<ClienteDto>>
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public Handler(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ClienteDto>> Handle(ListarClientesQuery request, CancellationToken ct)
        {
            var clientes = await _repo.ListAsync(ct);
            return _mapper.Map<IReadOnlyList<ClienteDto>>(clientes);
        }
    }
}