using System.Collections.Generic;
using System.Threading.Tasks;
using MauiApp4.Models.Entities;
using MauiApp4.Services.Persistence;

namespace MauiApp4.Models.Business
{
    public class UserService
    {
        private readonly IUserDao _userDao;

        public UserService(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync() => _userDao.GetAllAsync();

        public Task<User> CreateUserAsync(string name, string email)
        {
            var user = new User { Name = name, Email = email };
            return _userDao.AddAsync(user);
        }

        public Task DeleteUserAsync(int id) => _userDao.DeleteAsync(id);

        // Convenience wrapper to get a user by id
        public Task<User?> GetByIdAsync(int id) => _userDao.GetByIdAsync(id);
    }
}
