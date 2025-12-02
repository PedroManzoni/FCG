using Fcg.Domain.Interfaces;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fcg.Domain.UserHandlers;

public class DeleteUserHandler : IRequestHandler<DeleteUserRequest, DeleteUserResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteUserHandler> _logger;
    public DeleteUserHandler(IUserRepository userRepository, ILogger<DeleteUserHandler> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }
    public async Task<DeleteUserResponse> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tentativa de deleção do usuário: {Email}", request.email);
        var user = await _userRepository.GetUserByEmailAsync(request.email);
        if (user is null)
        {
            _logger.LogWarning("Falha na deleção: Usuário não encontrado - {Email}", request.email);
            throw new KeyNotFoundException($"Usuário '{request.email}' não encontrado.");
        }
        await _userRepository.DeleteUserAsync(user.Email);

        _logger.LogInformation("Usuário deletado com sucesso: {Email}", request.email);
        return new DeleteUserResponse("Usuário deletado com sucesso");
    }
}
