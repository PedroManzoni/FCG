using Fcg.Domain.Entities;
using Fcg.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Data.Repository;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Game> _dbSet;

    public GameRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Game>();
    }

    public async Task CreateGameAsync(Game game)
    {
        await _dbSet.AddAsync(game);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteGameAsync(string name)
    {
        var game = await _dbSet.FirstOrDefaultAsync(u => u.Name == name);

        if (game is null)
            return;

        _dbSet.Remove(game);
        await _context.SaveChangesAsync();
    }

    public async Task<IList<Game>> GetAllGamesAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<Game?> GetGameByNameAsync(string name)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Name == name);
    }

    public async Task UpdateGameAsync(Game game)
    {
        _dbSet.Update(game);
        await _context.SaveChangesAsync();
    }
}
