using Fcg.Domain.Entities;
using Fcg.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fcg.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<User> _dbSet;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<User>();
    }

    public async Task CreateUserAsync(User user)
    {
        await _dbSet.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string email)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email);

        if (user is null)
            return;

        _dbSet.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<IList<User>> GetAllUsersAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task UpdateUserAsync(User user)
    {
        _dbSet.Update(user);
        await _context.SaveChangesAsync();
    }
}
