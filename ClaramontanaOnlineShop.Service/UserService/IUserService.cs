using ClaramontanaOnlineShop.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.Service.UserService
{
    public interface IUserService
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(Guid userId);
        Task<User> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
    }
}