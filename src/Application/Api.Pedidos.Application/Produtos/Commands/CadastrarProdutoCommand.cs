using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Abstractions;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Api.Pedidos.Application.Produtos.Commands;

public class CadastrarProdutoCommand : IRequest<ProdutoCadastroDto>
{
    public string Nome { get; set; }
    public string? Descricao { get; set; }
    public  decimal Preco { get; set; }
    public decimal PrecoVenda { get; set; }
    public DateTime DataCadastro { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public bool IsAtivo { get; set; }

    public CadastrarProdutoCommand( string nome, string descricao, decimal preco, decimal precoVenda,  DateTime dataCadastro,  DateTime dataAtualizacao, bool isAtivo)
    {
        Nome = nome;
        Descricao = descricao;
        Preco = preco;
        PrecoVenda = precoVenda;
        DataCadastro = dataCadastro;
        DataAtualizacao = dataAtualizacao;
        IsAtivo = isAtivo;
    }
}

public class Handler : IRequestHandler<CadastrarProdutoCommand, ProdutoCadastroDto>
{
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProdutoRepository _produtoRepo;
    private readonly IMapper _mapper;


    public Handler(IUnitOfWork unitOfWork, IProdutoRepository produtoRepo, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _produtoRepo = produtoRepo;
        _mapper = mapper;
    }
    
    public async Task<ProdutoCadastroDto> Handle(CadastrarProdutoCommand request, CancellationToken ct)
    {

        if (request.PrecoVenda < request.Preco)
        {
            throw new Exception("Preço de venda invalido");
        }
        
        if (request.PrecoVenda < 0 || request.Preco < 0)
        {
            throw new Exception("Preço informado invalido");
        }
        
       var produto = Produto.Criar(request.Nome, request.Descricao, request.Preco,  request.PrecoVenda,request.DataCadastro, request.DataAtualizacao, request.IsAtivo );
       
       await _produtoRepo.AddAsync(produto, ct);
       await _unitOfWork.SaveChangesAsync();
       
    
        return _mapper.Map<Produto, ProdutoCadastroDto>(produto);   
    }
}