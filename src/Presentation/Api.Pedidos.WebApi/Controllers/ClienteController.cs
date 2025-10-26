using Api.Pedidos.Application.Clientes.Commands;
using Api.Pedidos.Application.Clientes.Queries;
using Api.Pedidos.Application.Common.Dtos;
using Api.Pedidos.Application.Common.Responses;
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
    // GET
    // =========================

    /// <summary>
    /// Lista todos os clientes.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClienteDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClienteDto>>>> Listar(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListarClientesQuery(), ct);
        return Ok(ApiResponse<IReadOnlyList<ClienteDto>>.Ok(result));
    }

    /// <summary>
    /// Lista clientes que possuem ao menos um pedido.
    /// </summary>
    [HttpGet("cliente-com-pedidos")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ClienteDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ClienteDto>>>> ListarComPedidos(CancellationToken ct = default)
    {
        var result = await _mediator.Send(new ListarClientesComPedidosQuery(), ct);
        return Ok(ApiResponse<IReadOnlyList<ClienteDto>>.Ok(result));
    }

    /// <summary>
    /// Obtém um cliente pelo seu identificador.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<Cliente>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ClienteDto>>> ObterPorId(int id, CancellationToken ct = default)
    {
        var cliente = await _mediator.Send(new ObterClientePorIdQuery { Id = id }, ct);
        return cliente is null
            ? NotFound(ApiResponse<object?>.Fail("Cliente não encontrado.", HttpContext.TraceIdentifier))
            : Ok(ApiResponse<Cliente>.Ok(cliente));
    }

    // =========================
    // POST
    // =========================

    /// <summary>
    /// Cadastra um novo cliente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object>>> Cadastrar([FromBody] CadastrarClienteRequest? body, CancellationToken ct = default)
    {
        if (body is null)
            return BadRequest(ApiResponse<object?>.Fail("Body inválido.", HttpContext.TraceIdentifier));
        
        var cmd = new CadastrarClienteCommand(
            nome: body.Nome,
            email: body.Email,
            telefone: body.Telefone,
            isAtivo: body.IsAtivo ?? true,
            endereco: body.Endereco
        );

        var id = await _mediator.Send(cmd, ct);

        var response = ApiResponse<object>.Ok(
            new { id },
            "Cliente criado com sucesso.",
            HttpContext.TraceIdentifier);

        return CreatedAtAction(nameof(ObterPorId), new { id }, response);
    }

    // =========================
    // PUT
    // =========================

    /// <summary>
    /// Edita os dados de um cliente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<ClienteDto?>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<ClienteDto?>>> Editar(int id, [FromBody] EditarClienteRequest? body, CancellationToken ct = default)
    {
        if (body is null)
            return BadRequest(ApiResponse<ClienteDto?>.Fail("Body inválido.", HttpContext.TraceIdentifier));

        var cmd = new EditarClienteCommand(
            id: id,
            nome: body.Nome,
            email: body.Email,
            telefone: body.Telefone,
            endereco: body.Endereco 
        );

        await _mediator.Send(cmd, ct);

        return Ok(ApiResponse<ClienteDto?>.Ok(null, "Cliente atualizado com sucesso.", HttpContext.TraceIdentifier));
    }

    // =========================
    // PATCH
    // =========================

    /// <summary>
    /// Inativa um cliente.
    /// </summary>
    [HttpPatch("{id:int}/inativar")]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object?>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object?>>> Inativar(int id, CancellationToken ct = default)
    {
        await _mediator.Send(new InativarClienteCommand { ClienteId = id }, ct);
        return Ok(ApiResponse<object?>.Ok(null, "Cliente inativado com sucesso.", HttpContext.TraceIdentifier));
    }
}