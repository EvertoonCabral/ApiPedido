using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Repositories;
using MediatR;

namespace Api.Pedidos.Application.Produtos.Commands;

public class EditarProdutoCommand : IRequest<object?>
{
    public int ProdutoId { get; set; }
    public string Nome { get; set; } = default!;
    public string Descricao { get; set; } = default!;
    public decimal Preco { get; set; }
    public decimal PrecoVenda { get; set; }

    public EditarProdutoCommand() { }

    public EditarProdutoCommand(int produtoId, string nome, string descricao, decimal preco, decimal precoVenda)
    {
        ProdutoId = produtoId;
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        PrecoVenda = precoVenda;
    }

    public class Handler : IRequestHandler<EditarProdutoCommand, object?>
    {
        private readonly IProdutoRepository _repo;
        private readonly IUnitOfWork _uow;

        public Handler(IProdutoRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<object?> Handle(EditarProdutoCommand request, CancellationToken ct)
        {
            var produto = await _repo.GetByIdAsync(request.ProdutoId, ct)
                          ?? throw new Exception("Produto não encontrado.");

            if (produto.IsAtivo == false)
            {
                throw new Exception($"Produto {produto.Nome} esta inativo e não pode ser editada.");
            }

            produto.Editar(request.Nome, request.Descricao, request.Preco, request.PrecoVenda);

            await _repo.UpdateAsync(produto, ct);
            await _uow.SaveChangesAsync(ct);
            return Unit.Value;
        }
    }
}