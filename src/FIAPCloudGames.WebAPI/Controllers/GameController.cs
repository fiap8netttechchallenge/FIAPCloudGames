using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.WebAPI.Contracts;
using FIAPCloudGames.WebAPI.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FIAPCloudGames.WebAPI.Controllers;

public class GameController : ApiController
{
    private readonly IGameService _gameService;
    private readonly IUserService _userService;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameService gameService, IUserService userService, ILogger<GameController> logger)
    {
        _gameService = gameService;
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Cria um novo jogo no sistema.
    /// </summary>
    /// <param name="createGameRequest">Dados do jogo a ser criado.</param>
    /// <returns>Retorna o jogo criado.</returns>
    /// <response code="201">Jogo criado com sucesso.</response>
    /// <response code="400">Erro de validação nos dados informados.</response>
    [Authorize(Roles = "admin")]
    [HttpPost(ApiRoutes.Games.Create)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateGameRequest createGameRequest)
    {
        _logger.LogInformation("Iniciando criação de jogo | CorrelationId: {CorrelationId}",
            HttpContext.TraceIdentifier);

        var validator = new CreateGameRequestValidator();
        var result = validator.Validate(createGameRequest);

        if (!result.IsValid)
        {
            _logger.LogWarning("Falha ao criar jogo | CorrelationId: {CorrelationId} | Erro: {result}",
                HttpContext.TraceIdentifier, result.ToDictionary());

            return BadRequest(result.ToDictionary());
        }

        var emailClaim = User.FindFirst(ClaimTypes.Email);
        if (emailClaim == null)
        {
            return BadRequest("E-mail não encontrado no token.");
        }
        var email = emailClaim.Value;

        var user = await _userService.GetByEmail(email);
        if (user == null)
        {
            _logger.LogWarning("Usuário não encontrado | CorrelationId: {CorrelationId} | Email: {Email}",
                HttpContext.TraceIdentifier, email);
            return BadRequest("Usuário não encontrado.");
        }

        var userId = user.Id;
        createGameRequest.UserId = userId;

        var game = await _gameService.Create(createGameRequest);

        _logger.LogInformation("Jogo criado com sucesso | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, game.Id);

        return Ok(game);
    }

    /// <summary>
    /// Atualiza os dados de um jogo existente.
    /// </summary>
    /// <param name="gameId">ID do jogo.</param>
    /// <param name="updateGameRequest">Dados atualizados do jogo.</param>
    /// <returns>Confirmação da atualização.</returns>
    /// <response code="200">Jogo atualizado com sucesso.</response>
    /// <response code="400">Erro nos dados informados.</response>
    [Authorize(Roles = "admin")]
    [HttpPut(ApiRoutes.Games.Update)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int gameId, [FromBody] UpdateGameRequest updateGameRequest)
    {
        _logger.LogInformation("Iniciando atualização de jogo | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        updateGameRequest.Id = gameId;

        var validator = new UpdateGameRequestValidator();
        var result = validator.Validate(updateGameRequest);
        if (!result.IsValid)
        {
            _logger.LogWarning("Falha ao atualizar o jogo | CorrelationId: {CorrelationId} | GameId: {GameId} | Erro: {result}",
                HttpContext.TraceIdentifier, gameId, result.ToDictionary());

            return BadRequest(result.ToDictionary());
        }

        var userId = int.Parse(User.Claims.ToList()[0].Value);
        updateGameRequest.UserId = userId;

        await _gameService.Update(updateGameRequest);

        _logger.LogInformation("Jogo atualizado com sucesso | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        return Ok();
    }

    /// <summary>
    /// Obtém os dados de um jogo pelo ID.
    /// </summary>
    /// <param name="gameId">ID do jogo.</param>
    /// <returns>Dados do jogo.</returns>
    /// <response code="200">Jogo encontrado.</response>
    /// <response code="404">Jogo não encontrado.</response>
    [Authorize(Roles = "admin,user")]
    [HttpGet(ApiRoutes.Games.GetById)]
    [ProducesResponseType(typeof(GameResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int gameId)
    {
        _logger.LogInformation("Buscando jogo por ID | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        var result = await _gameService.GetById(gameId);

        if (result == null)
        {
            _logger.LogWarning("Jogo não encontrado | CorrelationId: {CorrelationId} | GameId: {GameId}",
                HttpContext.TraceIdentifier, gameId);

            return NotFound();
        }

        _logger.LogInformation("Jogo encontrado | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        return Ok(result);
    }

    /// <summary>
    /// Obtém a lista de todos os jogos.
    /// </summary>
    /// <returns>Lista de jogos.</returns>
    /// <response code="200">Lista retornada com sucesso.</response>
    /// <response code="404">Nenhum jogo encontrado.</response>
    [Authorize(Roles = "admin,user")]
    [HttpGet(ApiRoutes.Games.GetAll)]
    [ProducesResponseType(typeof(IEnumerable<GameResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAll()
    {
        _logger.LogInformation("Iniciando recuperação de todos os jogos | CorrelationId: {CorrelationId}",
            HttpContext.TraceIdentifier);

        var result = await _gameService.GetAll();

        _logger.LogInformation("Lista de jogos recuperada com sucesso | CorrelationId: {CorrelationId} | Quantidade: {Count}",
            HttpContext.TraceIdentifier, result.Count());

        return Ok(result);
    }

    /// <summary>
    /// Ativa um jogo.
    /// </summary>
    /// <param name="gameId">ID do jogo a ser ativado.</param>
    /// <returns>Confirmação da ativação.</returns>
    /// <response code="200">Jogo ativado com sucesso.</response>
    /// <response code="400">Erro ao ativar jogo.</response>
    [Authorize(Roles = "admin")]
    [HttpPut(ApiRoutes.Games.Active)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Active(int gameId)
    {
        _logger.LogInformation("Iniciando ativação de jogo | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        await _gameService.Active(gameId);

        _logger.LogInformation("Jogo ativado com sucesso | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        return Ok();
    }

    /// <summary>
    /// Inativa um Jogo.
    /// </summary>
    /// <param name="gameId">ID do jogo a ser inativado.</param>
    /// <returns>Confirmação da inativação.</returns>
    /// <response code="200">Jogo inativado com sucesso.</response>
    /// <response code="400">Erro ao inativar jogo.</response>
    [Authorize(Roles = "admin")]
    [HttpPut(ApiRoutes.Games.Inactive)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Inactive(int gameId)
    {
        _logger.LogInformation("Iniciando inativação de jogo | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        await _gameService.Inactive(gameId);

        _logger.LogInformation("Jogo inativado com sucesso | CorrelationId: {CorrelationId} | GameId: {GameId}",
            HttpContext.TraceIdentifier, gameId);

        return Ok();
    }
}
