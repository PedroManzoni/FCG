using Fcg.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fcg.Domain.Interfaces;

public interface IUserRepository
{
    Task CreateUserAsync(User user);

    Task<User?> GetUserByIdAsync(Guid id);

    Task<IList<User>> GetAllUsersAsync();

    Task<User?> GetUserByEmailAsync(string email);

    Task UpdateUserAsync(User user);

    Task DeleteUserAsync(string email);
}
