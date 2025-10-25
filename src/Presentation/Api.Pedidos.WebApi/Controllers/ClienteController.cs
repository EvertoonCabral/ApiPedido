using Api.Pedidos.Application.Clientes.Commands;
using Api.Pedidos.Application.Clientes.Queries;
using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Domain.Models;
using Api.Pedidos.WebApi.Contracts.Request.Clientes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Pedidos.WebApi.Controllers;

[ApiController]
[Route("api/clientes")]
[Produces("application/json")]
public class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // =========================
    // POST
    // =========================

    /// <summary>
    /// Cadastra um novo cliente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<int>> Cadastrar([FromBody] CadastrarClienteRequest? body, CancellationToken ct = default)
    {
        if (body is null)
            return BadRequest("Body inválido.");

        if (string.IsNullOrWhiteSpace(body.Nome))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(body.Email))
            return BadRequest("Email é obrigatório.");

        if (string.IsNullOrWhiteSpace(body.Telefone))
            return BadRequest("Telefone é obrigatório.");

        var cmd = new CadastrarClienteCommand(
            nome: body.Nome,
            email: body.Email,
            telefone: body.Telefone,
            isAtivo: body.IsAtivo ?? true,
            endereco: body.Endereco
        );

        var id = await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id }, id);
    }


    // =========================
    // GET
    // =========================

    /// <summary>
    /// Lista todos os clientes.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClienteDto>>> Listar(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListarClientesQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Lista clientes que possuem ao menos um pedido.
    /// </summary>
    [HttpGet("cliente-com-pedidos")]
    [ProducesResponseType(typeof(IReadOnlyList<ClienteDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ClienteDto>>> ListarComPedidos(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListarClientesComPedidosQuery(), ct);
        return Ok(result);
    }

    /// <summary>
    /// Obtém um cliente pelo seu identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClienteDto>> ObterPorId(int id, CancellationToken ct = default)
    {
        var dto = await _mediator.Send(new ObterClientePorIdQuery { ClienteId = id }, ct);
        return dto is null ? NotFound() : Ok(dto);
    }


    // =========================
    // PUT
    // =========================

    /// <summary>
    /// Edita os dados de um cliente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Editar(int id, [FromBody] EditarClienteRequest? body, CancellationToken ct = default)
    {
        if (body is null)
            return BadRequest("Body inválido.");

        if (string.IsNullOrWhiteSpace(body.Nome))
            return BadRequest("Nome é obrigatório.");

        if (string.IsNullOrWhiteSpace(body.Email))
            return BadRequest("Email é obrigatório.");

        if (string.IsNullOrWhiteSpace(body.Telefone))
            return BadRequest("Telefone é obrigatório.");

        var cmd = new EditarClienteCommand(
            nome: body.Nome,
            email: body.Email,
            telefone: body.Telefone,
            endereco: body.Endereco 
        );

        await _mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id }, id);
    }

    // =========================
    // PATCH
    // =========================

    /// <summary>
    /// Inativa um cliente.
    /// </summary>
    [HttpPatch("{id:int}/inativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Inativar(int id, CancellationToken ct = default)
    {
        await _mediator.Send(new InativarClienteCommand { ClienteId = id }, ct);
        return NoContent();
    }
}



