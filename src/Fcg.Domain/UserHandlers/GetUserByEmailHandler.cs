using Fcg.Domain.Interfaces;
using Fcg.Shareable.Dtos;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fcg.Domain.UserHandlers;

public class GetUserByEmailHandler : IRequestHandler<GetUserByEmailRequest, GetUserByEmailResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<GetUserByEmailHandler> _logger;
    public GetUserByEmailHandler(IUserRepository userRepository, ILogger<GetUserByEmailHandler> logger)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<GetUserByEmailResponse> Handle(GetUserByEmailRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando usuário pelo email: {Email}", request.email);
        var user = await _userRepository.GetUserByEmailAsync(request.email);
        if (user is null)
        {
            _logger.LogWarning("Usuário não encontrado: {Email}", request.email);
            throw new KeyNotFoundException($"Usuário '{request.email}' não encontrado.");
        }

        var userDto = new UserDto
        {
            Email = user.Email,
            Name = user.Name
        };

        _logger.LogInformation("Usuário encontrado com sucesso: {Email}", request.email);
        return new GetUserByEmailResponse(userDto);
    }
}
