using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Commands;

public class EditarClienteCommand : IRequest<Unit>
{
    public int ClienteId { get; set; }
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Telefone { get; set; } = default!;
    public Endereco? Endereco { get; set; }

    public EditarClienteCommand() { }

    public EditarClienteCommand(int clienteId, string nome, string email, string telefone, Endereco? endereco)
    {
        ClienteId = clienteId;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Endereco = endereco;
    }

    public class Handler : IRequestHandler<EditarClienteCommand, Unit>
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public Handler(IClienteRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<Unit> Handle(EditarClienteCommand request, CancellationToken ct)
        {
            var cliente = await _repo.GetByIdAsync(request.ClienteId, ct)
                          ?? throw new Exception("Cliente não encontrado.");


            cliente.Editar(request.Nome, request.Email, request.Telefone, request.Endereco);

            await _repo.UpdateAsync(cliente);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}