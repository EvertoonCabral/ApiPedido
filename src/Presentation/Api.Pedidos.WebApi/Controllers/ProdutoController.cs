using Api.Pedidos.Application.Common.Responses;
using Api.Pedidos.Application.Produtos.Commands;
using Api.Pedidos.Application.Produtos.Queries;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.WebApi.Contracts.Request.Produtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pedidos.WebApi.Controllers;

[ApiController]
[Route("api/produtos")]
[Produces("application/json")]
public class ProdutoController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProdutoController(IMediator mediator)
    {
        _mediator = mediator;
    }


    /// <summary>
    /// Cadastrar um novo Produto.
    /// </summary>
    [HttpPost("cadastrar-produto")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarProdutoRequest request,
        CancellationToken ct = default)
    {


        var cmd = new CadastrarProdutoCommand(
            nome: request.Nome,
            descricao: request.Descricao,
            preco: request.Preco,
            precoVenda: request.PrecoVenda,
            dataCadastro: DateTime.Now,
            dataAtualizacao: DateTime.Now,
            isAtivo: true
        );

        var produto = await _mediator.Send(cmd, ct);
        if (produto is null)
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object?>.Fail("Falha ao criar o produto.", HttpContext.TraceIdentifier));

        var response = ApiResponse<object>.Ok(
            new { id = produto.Id },
            "Produto criado com sucesso.",
            HttpContext.TraceIdentifier
        );

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = produto.Id },
            response
        );
    }


    /// <summary>
    /// Obtém um cliente pelo seu identificador.
    /// </summary>
    [HttpGet("{produtoId:int}")]
    [ProducesResponseType(typeof(ApiResponse<Produto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Produto>>> ObterPorId(int produtoId, CancellationToken ct = default)
    {
        var query = new ObterProdutoPorIdQuery { ProdutoId = produtoId };
        
        var result = await _mediator.Send(query , ct);
        
        if (result is null)
            return NotFound();
        
        return Ok(ApiResponse<Produto>.Ok(result));
    }

    /// <summary>
    /// Lista todos os produtos.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Produto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Produto>>> Listar(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListarProdutosQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Edita os dados de um produto.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Editar(int id, [FromBody] EditarProdutoRequest request,
        CancellationToken ct = default)
    {
        var cmd = new EditarProdutoCommand(
            produtoId: id,
            nome: request.Nome,
            descricao: request.Descricao,
            preco: request.Preco,
            precoVenda: request.PrecoVenda
        );
        var produtoDto = await _mediator.Send(cmd, ct);
        
        return Ok(produtoDto);
    }

    /// <summary>
    /// Inativa um produto.
    /// </summary>
    [HttpPatch("{produtoId:int}/inativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Inativar(int produtoId, CancellationToken ct = default)
    {
        var query = new InativarProdutoCommand { ProdutoId = produtoId };

        await _mediator.Send(query, ct);
        
        return Ok(new { mensagem = $"Produto de Id {produtoId} foi inativado." });
    }

    /// <summary>
    /// Ativa um produto.
    /// </summary>
    [HttpPatch("{produtoId:int}/ativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Ativar(int produtoId, CancellationToken ct = default)
    {
        var query = new AtivarProdutoCommand { ProdutoId = produtoId };

        await _mediator.Send(query, ct);
        
        return Ok(new { mensagem = $"Produto de Id {produtoId} foi ativado." });
    }
}