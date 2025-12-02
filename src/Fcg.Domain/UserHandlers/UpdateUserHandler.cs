using Fcg.Domain.Interfaces;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fcg.Domain.UserHandlers;

public class UpdateUserHandler : IRequestHandler<UpdateUserRequest, UpdateUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IUserRepository userRepository, ILogger<UpdateUserHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UpdateUserResponse> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tentativa de atualização do usuário: {Email}", request.email);
        var user = await _userRepository.GetUserByEmailAsync(request.email);
        if (user is null)
        {
            _logger.LogWarning("Falha na atualização: Usuário não encontrado - {Email}", request.email);
            throw new KeyNotFoundException($"Usuário '{request.email}' não encontrado.");
        }

        _logger.LogInformation("Atualizando nome do usuário: {Email}", request.email);
        user.Name = string.IsNullOrWhiteSpace(request.user.Name)
            ? user.Name
            : request.user.Name;
        

        if (!string.IsNullOrWhiteSpace(request.user.Password))
        {
            _logger.LogInformation("Atualizando senha do usuário: {Email}", request.email);
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.user.Password);
        }

        user.LastUpdatedAt = DateTime.UtcNow;


        await _userRepository.UpdateUserAsync(user);

        _logger.LogInformation("Usuário atualizado com sucesso: {Email}", request.email);

        return new UpdateUserResponse("Usuário alterado com sucesso");
    }
}
