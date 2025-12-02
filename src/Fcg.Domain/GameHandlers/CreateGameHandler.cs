using Fcg.Domain.Entities;
using Fcg.Domain.Interfaces;
using Fcg.Shareable.Requests;
using Fcg.Shareable.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fcg.Domain.GameHandlers;

public class CreateGameHandler : IRequestHandler<CreateGameRequest, CreateGameResponse>
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<CreateGameHandler> _logger;

    public CreateGameHandler(IGameRepository gameRepository, ILogger<CreateGameHandler> logger)
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task<CreateGameResponse> Handle(CreateGameRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando criação de jogo: {GameName}", request.Game.Name);
        var existingGame = await _gameRepository.GetGameByNameAsync(request.Game.Name);

        if (existingGame is not null)
        {
            _logger.LogWarning("Tentativa de cadastrar jogo duplicado: {GameName}", request.Game.Name);
            throw new ArgumentException($"O jogo '{request.Game.Name}' já está cadastrado.");
        }

        var game = new Game
        {
            Name = request.Game.Name,
            Description = request.Game.Description,
            Price = request.Game.Price,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow
        };
        await _gameRepository.CreateGameAsync(game);

        _logger.LogInformation("Jogo criado com sucesso: {GameName} | Preço: {Price:C}", game.Name, game.Price);

        return new CreateGameResponse("Jogo cadastrado com sucesso");
    }
}
