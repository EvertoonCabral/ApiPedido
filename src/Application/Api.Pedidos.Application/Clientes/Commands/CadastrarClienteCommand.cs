using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Clientes.Commands;

public class CadastrarClienteCommand : IRequest<int>
{
    public string Nome { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Telefone { get; set;} = default!;
    public bool IsAtivo { get; set; } = true;
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

        public async Task<int> Handle(CadastrarClienteCommand request, CancellationToken cancellationToken)
        {


            var cliente = Cliente.Criar(request.Nome, request.Email, request.Telefone, request.Endereco, request.IsAtivo);

            await _clienteRepo.AddAsync(cliente, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return cliente.Id;
        }
    }
}