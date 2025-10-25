using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Commands;

public class CadastrarClienteCommand : IRequest<int>
{
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Telefone { get; set;}
    public bool IsAtivo { get; set; }
    public Endereco? Endereco { get; set; }
    

    public CadastrarClienteCommand(string nome, string email, string telefone, bool isAtivo, Endereco? endereco)
    {
        Nome = nome;
        Email = email;
        Telefone = telefone;
        IsAtivo = isAtivo;
        Endereco = endereco;
    }

    public class Handler : IRequestHandler<CadastrarClienteCommand, int>
    {
        private readonly IClienteRepository _clienteRepo;
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IClienteRepository clienteRepo, IUnitOfWork unitOfWork)
        {
            _clienteRepo = clienteRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(CadastrarClienteCommand request, CancellationToken ct)
        {
            
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

            var cliente = Cliente.Criar(request.Nome, request.Email, request.Telefone, endereco, request.IsAtivo);

            await _clienteRepo.AddAsync(cliente, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return cliente.Id;
        }
    }
}