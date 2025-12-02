using Fcg.Domain.Entities;
using Fcg.Domain.Interfaces;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fcg.Domain.UserHandlers;

public class CreateUserHandler : IRequestHandler<CreateUserRequest, CreateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IUserRepository userRepository, ILogger<CreateUserHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    public async Task<CreateUserResponse> Handle(CreateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tentativa de cadastro de usuário: {Email}", request.User.Email);
        var existingUser = await _userRepository.GetUserByEmailAsync(request.User.Email);

        if (existingUser is not null)
        {
            _logger.LogWarning("Falha no cadastro: Email já cadastrado - {Email}", request.User.Email);
            throw new ArgumentException($"O email '{request.User.Email}' já está cadastrado.");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.User.Name,
            Email = request.User.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.User.Password),
            Role = "User",
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };

        await _userRepository.CreateUserAsync(user);

        _logger.LogInformation("Usuário cadastrado com sucesso: {Email}", user.Email);
        return new CreateUserResponse("Usuário cadastrado com sucesso");
        
       
    }
};
