using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.WebAPI.Contracts;
using FIAPCloudGames.WebAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.WebAPI.Controllers;

[Authorize("admin")]
public class UserController : ApiController
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo usuário no sistema.
    /// </summary>
    /// <param name="createUserRequest">Dados do usuário a ser criado.</param>
    /// <returns>Retorna o usuário criado.</returns>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Erro de validação nos dados informados.</response>
    [AllowAnonymous]
    [HttpPost(ApiRoutes.Users.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest createUserRequest)
    {
        _logger.LogInformation("Iniciando criação de usuário | CorrelationId: {CorrelationId}",
            HttpContext.TraceIdentifier);

        var validator = new CreateUserRequestValidator();
        var result = validator.Validate(createUserRequest);

        if (!result.IsValid)
        {
            _logger.LogWarning("Falha ao criar usuário | CorrelationId: {CorrelationId} | Erro: {result}",
                HttpContext.TraceIdentifier, result.ToDictionary());

            return BadRequest(result.ToDictionary());
        }

        var user = await _userService.Create(createUserRequest);

        _logger.LogInformation("Usuário criado com sucesso | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, user.Id);

        return Ok(user);
    }

    /// <summary>
    /// Atualiza os dados de um usuário existente.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <param name="updateUserRequest">Dados atualizados do usuário.</param>
    /// <returns>Confirmação da atualização.</returns>
    /// <response code="200">Usuário atualizado com sucesso.</response>
    /// <response code="400">Erro nos dados informados.</response>
    [HttpPut(ApiRoutes.Users.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int userId, [FromBody] UpdateUserRequest updateUserRequest)
    {
        _logger.LogInformation("Iniciando atualização de usuário | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        updateUserRequest.Id = userId;

        var validator = new UpdateUserRequestValidator();
        var result = validator.Validate(updateUserRequest);
        if (!result.IsValid)
        {
            _logger.LogWarning("Falha ao atualizar usuário | CorrelationId: {CorrelationId} | UserId: {UserId} | Erro: {result}",
                HttpContext.TraceIdentifier, userId, result.ToDictionary());

            return BadRequest(result.ToDictionary());
        }

        await _userService.Update(updateUserRequest);

        _logger.LogInformation("Usuário atualizado com sucesso | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        return Ok();
    }

    /// <summary>
    /// Obtém os dados de um usuário pelo ID.
    /// </summary>
    /// <param name="userId">ID do usuário.</param>
    /// <returns>Dados do usuário.</returns>
    /// <response code="200">Usuário encontrado.</response>
    /// <response code="404">Usuário não encontrado.</response>
    [HttpGet(ApiRoutes.Users.GetById)]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int userId)
    {
        _logger.LogInformation("Buscando usuário por ID | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        var result = await _userService.GetById(userId);

        if (result == null)
        {
            _logger.LogWarning("Usuário não encontrado | CorrelationId: {CorrelationId} | UserId: {UserId}",
                HttpContext.TraceIdentifier, userId);
            return NotFound("Usuário não encontrado");
        }

        _logger.LogInformation("Usuário encontrado | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        return Ok(result);
    }

    /// <summary>
    /// Obtém a lista de todos os usuários.
    /// </summary>
    /// <returns>Lista de usuários.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="404">Nenhum usuário encontrado.</response>
    [HttpGet(ApiRoutes.Users.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Iniciando recuperação de todos os usuários | CorrelationId: {CorrelationId}",
            HttpContext.TraceIdentifier);

        var result = await _userService.GetAll();

        _logger.LogInformation("Lista de usuários recuperada com sucesso | CorrelationId: {CorrelationId} | Quantidade: {Count}",
            HttpContext.TraceIdentifier, result.Count());

        return Ok(result);
    }

    /// <summary>
    /// Ativa um usuário.
    /// </summary>
    /// <param name="userId">ID do usuário a ser ativado.</param>
    /// <returns>Confirmação da ativação.</returns>
    /// <response code="200">Usuário ativado com sucesso.</response>
    /// <response code="400">Erro ao ativar usuário.</response>
    [HttpPut(ApiRoutes.Users.Active)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Active(int userId)
    {
        _logger.LogInformation("Iniciando ativação de usuário | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        await _userService.Active(userId);

        _logger.LogInformation("Usuário ativado com sucesso | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        return Ok();
    }

    /// <summary>
    /// Inativa um usuário.
    /// </summary>
    /// <param name="userId">ID do usuário a ser inativado.</param>
    /// <returns>Confirmação da inativação.</returns>
    /// <response code="200">Usuário inativado com sucesso.</response>
    /// <response code="400">Erro ao inativar usuário.</response>
    [HttpPut(ApiRoutes.Users.Inactive)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Inactive(int userId)
    {
        _logger.LogInformation("Iniciando inativação de usuário | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        await _userService.Inactive(userId);

        _logger.LogInformation("Usuário inativado com sucesso | CorrelationId: {CorrelationId} | UserId: {UserId}",
            HttpContext.TraceIdentifier, userId);

        return Ok();
    }
}
