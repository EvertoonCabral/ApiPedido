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

    public EditarClienteCommand( string nome, string email, string telefone, Endereco? endereco)
    {
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
            
            
            Endereco? endereco = null;
            if (request.Endereco is not null)
            {
                endereco = new Endereco(
                    request.Endereco.Rua,
                    request.Endereco.Numero,
                    request.Endereco.Bairro,
                    request.Endereco.Cidade,
                    request.Endereco.Estado,
                    request.Endereco.Cep
                );
            }

            cliente.Editar(request.Nome, request.Email, request.Telefone, endereco);

            await _repo.UpdateAsync(cliente);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}