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

public class DeleteGameHandler : IRequestHandler<DeleteGameRequest, DeleteGameResponse> 
{
    private readonly IGameRepository _gameRepository;
    private readonly ILogger<DeleteGameHandler> _logger;

    public DeleteGameHandler(IGameRepository gameRepository, ILogger<DeleteGameHandler> logger)
    {
        _gameRepository = gameRepository;
        _logger = logger;
    }

    public async Task<DeleteGameResponse> Handle(DeleteGameRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando deleção de jogo: {GameName}", request.name);  
        var existingGame = await _gameRepository.GetGameByNameAsync(request.name);
        if (existingGame is null)
        {
            _logger.LogWarning("Tentativa de deletar jogo inexistente: {GameName}", request.name);  
            throw new KeyNotFoundException($"Jogo '{request.name}' não encontrado.");
        }
        await _gameRepository.DeleteGameAsync(request.name);

        _logger.LogInformation("Jogo deletado com sucesso: {GameName}", request.name);
        return new DeleteGameResponse("Jogo deletado com sucesso");
    }
}
