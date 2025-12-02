using Fcg.Domain.Interfaces;
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

public class UpdateGameHandler : IRequestHandler<UpdateGameRequest, UpdateGameResponse>
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<UpdateGameHandler> _logger;

    public UpdateGameHandler(IGameRepository gameRepository, ILogger<UpdateGameHandler> logger)
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task<UpdateGameResponse> Handle(UpdateGameRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando atualização do jogo: {GameName}", request.name);
        var existingGame = await _gameRepository.GetGameByNameAsync(request.name);
        if (existingGame is null)
        {
            _logger.LogWarning("Tentativa de atualizar jogo inexistente: {GameName}", request.name);
            throw new KeyNotFoundException($"Jogo '{request.name}' não encontrado.");
        }
        existingGame.Name = request.updatedGame.Name;
        existingGame.Description = request.updatedGame.Description;
        existingGame.Price = request.updatedGame.Price;
        existingGame.LastUpdatedAt = DateTime.UtcNow;
        await _gameRepository.UpdateGameAsync(existingGame);

        _logger.LogInformation("Jogo atualizado com sucesso: {GameName}", request.name);
        return new UpdateGameResponse("Jogo atualizado com sucesso");
    }

}
