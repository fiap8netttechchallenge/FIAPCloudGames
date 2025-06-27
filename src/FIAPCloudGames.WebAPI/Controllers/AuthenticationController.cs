using FIAPCloudGames.Application.Requests;
using FIAPCloudGames.Application.Responses;
using FIAPCloudGames.Application.Services.Interfaces;
using FIAPCloudGames.WebAPI.Contracts;
using FIAPCloudGames.WebAPI.Validators;
using Microsoft.AspNetCore.Mvc;

namespace FIAPCloudGames.WebAPI.Controllers;

public class AuthenticationController : ApiController
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Realiza o login do usuário com e-mail e senha.
    /// </summary>
    /// <param name="loginRequest">Dados de autenticação (e-mail e senha).</param>
    /// <returns>Retorna o token JWT em caso de sucesso.</returns>
    /// <response code="200">Login realizado com sucesso. Retorna o token JWT.</response>
    /// <response code="400">Erro de validação ou credenciais inválidas.</response>
    [HttpPost(ApiRoutes.Authentication.Login)]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        _logger.LogInformation("Tentativa de login iniciada | CorrelationId: {CorrelationId} | Email: {Email}",
            HttpContext.TraceIdentifier, loginRequest.Email);

        var validator = new LoginRequestValidator();
        var result = validator.Validate(loginRequest);
        if (!result.IsValid)
        {
            _logger.LogWarning("Falha no login | CorrelationId: {CorrelationId} | Email: {Email}",
                HttpContext.TraceIdentifier, loginRequest.Email);

            return BadRequest(result.ToDictionary());
        }

        var login = await _userService.Login(loginRequest);

        _logger.LogInformation("Login realizado com sucesso | CorrelationId: {CorrelationId} | Email: {Email}",
            HttpContext.TraceIdentifier, loginRequest.Email);

        return Ok(login);
    }
}
