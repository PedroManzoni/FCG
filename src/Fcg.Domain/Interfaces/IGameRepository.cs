using Fcg.Domain.Entities;

namespace Fcg.Domain.Interfaces;

public interface IGameRepository
{
    Task CreateGameAsync(Game game);

    Task<Game?> GetGameByNameAsync(string name);

    Task<IList<Game>> GetAllGamesAsync();

    Task UpdateGameAsync(Game game);

    Task DeleteGameAsync(string name);
}
