using ClaramontanaBibliography.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service
{
    public class UserService : IUserService
    {
        private readonly LibraryContext _libraryContext;
        public UserService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task<User> CreateAsync(User user)
        {
            _libraryContext.Users.Add(user);
            await _libraryContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _libraryContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user;
        }

        public async Task<User> GetByIdAsync(Guid userId)
        {
            var user = await _libraryContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _libraryContext.Users.FirstOrDefaultAsync(x => x.UserName == username);
            return user;
        }
    }
}
