using Api.Pedidos.Application.Common.Dtos;
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

    /// <summary>
    /// Inicia um novo pedido para um cliente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> IniciarPedido([FromBody] IniciarPedidoRequest body, CancellationToken ct)
    {
        if (body is null || body.ClienteId <= 0)
            return BadRequest("ClienteId inválido.");

        var id = await _mediator.Send(new IniciarPedidoCommand(body.ClienteId), ct);
        return CreatedAtAction(nameof(ObterPorId), new { id }, id);
    }

    /// <summary>
    /// Adiciona um item ao pedido.
    /// </summary>
    [HttpPost("{id:int}/itens")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AdicionarItem(int id, [FromBody] AdicionarProdutoAoPedidoRequest body, CancellationToken ct)
    {
        if (body is null || body.ProdutoId <= 0 || body.Quantidade <= 0)
            return BadRequest("ProdutoId e Quantidade devem ser maiores que zero.");

        var ok = await _mediator.Send(new AdicionarProdutoAoPedidoCommand(id, body.ProdutoId, body.Quantidade), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Remove ou decrementa a quantidade de um item do pedido.
    /// </summary>
    [HttpDelete("{id:int}/itens")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoverItem(int id, [FromBody] RemoverProdutoDoPedidoRequest body, CancellationToken ct)
    {
        if (body is null || body.ProdutoId <= 0 || body.Quantidade <= 0)
            return BadRequest("ProdutoId e Quantidade devem ser maiores que zero.");

        var ok = await _mediator.Send(new RemoverProdutoDoPedidoCommand(id, body.ProdutoId, body.Quantidade), ct);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Fecha o pedido, desde que as regras de negócio permitam.
    /// </summary>
    [HttpPost("{id:int}/fechar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Fechar(int id, CancellationToken ct)
    {
        var ok = await _mediator.Send(new FecharPedidoCommand(id), ct);
        return ok ? NoContent() : BadRequest();
    }

    /// <summary>
    /// Cancela o pedido, se ainda estiver em status que permita cancelamento.
    /// </summary>
    [HttpPost("{id:int}/cancelar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancelar(int id, CancellationToken ct)
    {
        var ok = await _mediator.Send(new CancelarPedidoCommand(id), ct);
        return ok ? NoContent() : BadRequest();
    }

    /// <summary>
    /// Obtém um pedido pelo seu identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoDto>> ObterPorId(int id, CancellationToken ct)
    {
        var dto = await _mediator.Send(new ObterPedidoPorIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Lista pedidos com filtros e paginação.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PageResponse<PedidoDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<PageResponse<PedidoDto>>> Listar(
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

        return Ok(result);
    }
}