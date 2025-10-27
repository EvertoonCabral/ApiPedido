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
    
    // =========================
    // POST
    // =========================

    /// <summary>
    /// Cadastrar um novo Produto.
    /// </summary>
    [HttpPost("cadastrar-produto")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarProdutoRequest request, CancellationToken ct = default)
    {
        if (request is null)
            return BadRequest(ApiResponse<object?>.Fail("Body inválido.", HttpContext.TraceIdentifier));

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
    
    // =========================
    // GET
    // =========================

    /// <summary>
    /// Obtém um cliente pelo seu identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<Produto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<Produto>>> ObterPorId(int id, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ObterProdutoPorIdQuery(id) , ct);
        if (result is null)
            return NotFound();
        return Ok(ApiResponse<Produto>.Ok(result));
    }


}