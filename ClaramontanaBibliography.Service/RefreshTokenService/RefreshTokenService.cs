using ClaramontanaBibliography.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly LibraryContext _libraryContext;

        public RefreshTokenService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task CreateAsync(RefreshTokenDto refreshTokenDto)
        {
            refreshTokenDto.Id = Guid.NewGuid();
            var refreshToken = new RefreshToken
            {
                Id = refreshTokenDto.Id,
                Token = refreshTokenDto.Token,
                UserId = refreshTokenDto.UserId
            };

            _libraryContext.RefreshTokens.Add(refreshToken);

            await _libraryContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var refreshToken = await _libraryContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

            return refreshToken;
        }
    }
}
