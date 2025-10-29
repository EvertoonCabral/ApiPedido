using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Commands;

public class EditarClienteCommand : IRequest<Cliente>
{
    public int Id { get; }
    public string Nome { get; }
    public string Email { get; }
    public string Telefone { get; }
    public Endereco? Endereco { get; }

    public EditarClienteCommand(int id, string nome, string email, string telefone, Endereco? endereco)
    {
        Id = id;
        Nome = nome;
        Email = email;
        Telefone = telefone;
        Endereco = endereco;
    }
}
    public class Handler : IRequestHandler<EditarClienteCommand, Cliente>
    {
        private readonly IClienteRepository _repo;
        private readonly IUnitOfWork _uow;

        public Handler(IClienteRepository repo, IUnitOfWork uow)
        {
            _repo = repo; _uow = uow;
        }

        public async Task<Cliente> Handle(EditarClienteCommand request, CancellationToken ct)
        {
            var cliente = await _repo.GetByIdAsync(request.Id, ct)
                          ?? throw new Exception("Cliente não encontrado.");
            
            if (cliente.IsAtivo == false)
            {
                throw new Exception($"O Pedido não pode ser editado pois o cliente {request.Nome} esta inativo.");
            }
            
            Endereco? endereco = null;
            if (request.Endereco is not null)
            {
                endereco = new Endereco(
                    request.Endereco.Rua,
                    request.Endereco.Numero,
                    request.Endereco.Bairro,
                    request.Endereco.Cidade,
                    request.Endereco.Uf,
                    request.Endereco.Cep
                );
            }
            if (request.Endereco is not null)
            {
                var e = request.Endereco;
                if (!string.IsNullOrWhiteSpace(e.Uf) && e.Uf.Length > 2)
                    throw new ArgumentException("Estado deve ter 2 caracteres (UF).");
            }

            cliente.Editar(request.Nome, request.Email, request.Telefone, endereco);

             _repo.Update(cliente);
            await _uow.SaveChangesAsync(ct);
            return cliente;
        }
    }