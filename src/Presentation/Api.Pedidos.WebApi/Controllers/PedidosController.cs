using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Application.Common.Responses;
using Api.Pedidos.Application.Pedidos.Commands;
using Api.Pedidos.Application.Pedidos.Queries;
using Api.Pedidos.WebApi.Contracts.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pedidos.WebApi.Controllers;

[ApiController]
[Route("api/pedidos")]
[Produces("application/json")]
public class PedidosController : ControllerBase
{
    private readonly IMediator _mediator;

    public PedidosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // =========================
    // GET
    // =========================
    
    /// <summary>
    /// Lista pedidos com filtros e paginação.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PageResponse<PedidoDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PageResponse<PedidoDto>>>> Listar(
        [FromQuery] int? clienteId,
        [FromQuery] string? status,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        if (page <= 0) page = 1;
        if (pageSize <= 0) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        var result = await _mediator.Send(new ListarPedidosQuery
        {
            ClienteId = clienteId,
            Status = status,
            Page = page,
            PageSize = pageSize
        }, ct);

        return Ok(ApiResponse<PageResponse<PedidoDto>>.Ok(result));
    }

    /// <summary>
    /// Obtém um pedido pelo seu identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<PedidoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<PedidoDto>>> ObterPorId([FromRoute] int id, CancellationToken ct = default)
    {
        var dto = await _mediator.Send(new ObterPedidoPorIdQuery(id), ct);
        return dto is null
            ? NotFound(ApiResponse<object?>.Fail("Pedido não encontrado.", HttpContext.TraceIdentifier))
            : Ok(ApiResponse<PedidoDto>.Ok(dto));
    }
    
    // =========================
    // POST
    // =========================

    /// <summary>
    /// Inicia um novo pedido para um cliente.
    /// </summary>
    [HttpPost("iniciar-pedido")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object>>> IniciarPedido([FromBody] IniciarPedidoRequest? body, CancellationToken ct = default)
    {
        if (body is null || body.ClienteId <= 0)
            return BadRequest(ApiResponse<object?>.Fail("ClienteId inválido.", HttpContext.TraceIdentifier));

        var id = await _mediator.Send(new IniciarPedidoCommand(body.ClienteId), ct);

        var response = ApiResponse<object>.Ok(
            new { id },
            "Pedido iniciado com sucesso.",
            HttpContext.TraceIdentifier);

        return CreatedAtAction(nameof(ObterPorId), new { id }, response);
    }

    /// <summary>
    /// Adiciona um item ao pedido.
    /// </summary>
    [HttpPost("{id:int}/adicionar-produto")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> AdicionarItem([FromRoute] int id, [FromBody] AdicionarProdutoAoPedidoRequest? body, CancellationToken ct = default)
    {
        if (body is null || body.ProdutoId <= 0 || body.Quantidade <= 0)
            return BadRequest(ApiResponse<object?>.Fail("ProdutoId e Quantidade devem ser maiores que zero.", HttpContext.TraceIdentifier));

        var ok = await _mediator.Send(new AdicionarProdutoAoPedidoCommand(id, body.ProdutoId, body.Quantidade), ct);
        if (!ok)
            return NotFound(ApiResponse<object?>.Fail("Pedido ou produto não encontrado.", HttpContext.TraceIdentifier));

        return Ok(ApiResponse<object?>.Ok(null, "Produto adicionado ao pedido com sucesso.", HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Fecha o pedido, desde que as regras de negócio permitam.
    /// </summary>
    [HttpPost("{id:int}/fechar-pedido")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object?>>> Fechar([FromRoute] int id, CancellationToken ct = default)
    {
        var ok = await _mediator.Send(new FecharPedidoCommand(id), ct);
        if (!ok)
            return BadRequest(ApiResponse<object?>.Fail("Não foi possível fechar o pedido.", HttpContext.TraceIdentifier));

        return Ok(ApiResponse<object?>.Ok(null, "Pedido fechado com sucesso.", HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Cancela o pedido, se ainda estiver em status que permita cancelamento.
    /// </summary>
    [HttpPost("{id:int}/cancelar-pedido")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object?>>> Cancelar([FromRoute] int id, CancellationToken ct = default)
    {
        var ok = await _mediator.Send(new CancelarPedidoCommand(id), ct);
        if (!ok)
            return BadRequest(ApiResponse<object?>.Fail("Não foi possível cancelar o pedido.", HttpContext.TraceIdentifier));

        return Ok(ApiResponse<object?>.Ok(null, "Pedido cancelado com sucesso.", HttpContext.TraceIdentifier));
    }

    // =========================
    // DELETE
    // =========================

    /// <summary>
    /// Remove ou decrementa a quantidade de um item do pedido.
    /// </summary>
    [HttpDelete("{id:int}/itens")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> RemoverItem([FromRoute] int id, [FromBody] RemoverProdutoDoPedidoRequest? body, CancellationToken ct = default)
    {
        if (body is null || body.ProdutoId <= 0 || body.Quantidade <= 0)
            return BadRequest(ApiResponse<object?>.Fail("ProdutoId e Quantidade devem ser maiores que zero.", HttpContext.TraceIdentifier));

        var ok = await _mediator.Send(new RemoverProdutoDoPedidoCommand(id, body.ProdutoId, body.Quantidade), ct);
        if (!ok)
            return NotFound(ApiResponse<object?>.Fail("Pedido ou item não encontrado.", HttpContext.TraceIdentifier));

        return Ok(ApiResponse<object?>.Ok(null, "Item removido do pedido com sucesso.", HttpContext.TraceIdentifier));
    }
}