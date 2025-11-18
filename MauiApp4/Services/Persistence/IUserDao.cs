using MauiApp4.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MauiApp4.Services.Persistence
{
    public interface IUserDao
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> AddAsync(User entity);
        Task<User?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<IEnumerable<User>> SearchByNameAsync(string query);
    }
}
