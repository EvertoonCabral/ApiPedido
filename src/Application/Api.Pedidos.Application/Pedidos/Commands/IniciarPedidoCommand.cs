using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Enums;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Pedidos.Commands
{
    public class IniciarPedidoCommand : IRequest<int>
    {
        public int ClienteId { get; set; }

        public IniciarPedidoCommand(int clienteId)
        {
            ClienteId = clienteId;
        }
    }

    public class IniciarPedidoCommandHandler : IRequestHandler<IniciarPedidoCommand, int>
    {
        private readonly IPedidoRepository _pedidoRepo;
        private readonly IClienteRepository _clienteRepo;
        private readonly IUnitOfWork _uow;

        public IniciarPedidoCommandHandler(
            IPedidoRepository pedidoRepo,
            IClienteRepository clienteRepo,
            IUnitOfWork uow)
        {
            _pedidoRepo = pedidoRepo;
            _clienteRepo = clienteRepo;
            _uow = uow;
        }

        public async Task<int> Handle(IniciarPedidoCommand request, CancellationToken cancellationToken)
        {
            var cliente = await _clienteRepo.GetByIdAsync(request.ClienteId, cancellationToken);
            if (cliente == null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            if (!cliente.IsAtivo)
                throw new InvalidOperationException("Não é possível iniciar pedido para cliente inativo.");

            var agora = DateTime.Now;

            var pedido = new Pedido
            {
                ClienteId = request.ClienteId,
                DataAbertura = agora,
                DataAtualizacao = agora,
                Status = StatusPedido.Aberto,
                Itens = new()
            };

            await _pedidoRepo.AddAsync(pedido, cancellationToken);
            await _uow.SaveChangesAsync(cancellationToken);

            return pedido.Id;
        }
    }
}