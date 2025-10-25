using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Models;
using AutoMapper;

namespace Api.Pedidos.Application.Pedidos.Mappings;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<ItemPedido, ItemPedidoDto>().ReverseMap();
        CreateMap<Cliente, ClienteDto>()
            .ForMember(dest => dest.EnderecoId, opt => opt.MapFrom(src => src.Endereco));
        CreateMap<Pedido, PedidoDto>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.Total, o => o.MapFrom(s => s.Itens.Sum(i => i.PrecoUnitario * i.Quantidade)));
    }
}