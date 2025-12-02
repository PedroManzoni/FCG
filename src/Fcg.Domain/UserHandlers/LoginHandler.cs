using Fcg.Domain.Interfaces;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fcg.Domain.UserHandlers;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(
        IUserRepository userRepository,
        ITokenService tokenService,
        ILogger<LoginHandler> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tentativa de login: {Email}", request.Login.Email);

        var user = await _userRepository.GetUserByEmailAsync(request.Login.Email);
        if (user == null)
        {
            _logger.LogWarning("Login falhou: Email não encontrado - {Email}", request.Login.Email);
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Login.Password, user.Password);
        if (!isPasswordValid)
        {
            _logger.LogWarning("Login falhou: Senha incorreta - {Email}", request.Login.Email);
            throw new UnauthorizedAccessException("Email ou senha inválidos.");
        }

        var token = _tokenService.GenerateToken(user);

        _logger.LogInformation("Login bem-sucedido: {Email} | Role: {Role}", user.Email, user.Role);

        return new LoginResponse(
            Success: true,
            Message: "Login realizado com sucesso",
            Token: token,
            Role: user.Role
        );
    }
}
