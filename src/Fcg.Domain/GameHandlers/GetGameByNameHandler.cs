using Fcg.Domain.Interfaces;
using Fcg.Shareable.Dtos;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fcg.Domain.GameHandlers;

public class GetGameByNameHandler : IRequestHandler<GetGameByNameRequest, GetGameByNameResponse>
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<GetGameByNameHandler> _logger;
    public GetGameByNameHandler(IGameRepository gameRepository, ILogger<GetGameByNameHandler> logger)
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }
    public async Task<GetGameByNameResponse> Handle(GetGameByNameRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Buscando jogo pelo nome: {Name}", request.name);
        var game = await _gameRepository.GetGameByNameAsync(request.name);
        if (game is null)
        {
            _logger.LogWarning("Jogo não encontrado: {Name}", request.name);
            throw new KeyNotFoundException($"Jogo '{request.name}' não encontrado.");
        }
        var gameDto = new GameDto
        {
            Name = game.Name,
            Description = game.Description,
            Price = game.Price
        };
        _logger.LogInformation("Jogo encontrado: {Name}", request.name);
        return new GetGameByNameResponse(gameDto);
    }
}
