using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Queries;

public class ListarClientesComPedidosQuery : IRequest<IReadOnlyList<ClienteDto>>
{
    public class Handler : IRequestHandler<ListarClientesComPedidosQuery, IReadOnlyList<ClienteDto>>
    {
        private readonly IClienteRepository _clienteRepo;
        private readonly IMapper _mapper;

        public Handler(IClienteRepository readRepo, IMapper mapper)
        {
        _clienteRepo = readRepo;
         _mapper = mapper;
        } 

        public async Task<IReadOnlyList<ClienteDto>> Handle(ListarClientesComPedidosQuery request, CancellationToken ct)
        {
            var clientes = await _clienteRepo.ListarComPedidosAsync(ct);
            return _mapper.Map<IReadOnlyList<ClienteDto>>(clientes);
        }
    }

}