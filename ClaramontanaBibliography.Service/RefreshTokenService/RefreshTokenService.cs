using ClaramontanaBibliography.Data;
using ClaramontanaBibliography.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service.RefreshTokenService
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ProductContext _dbContext;

        public RefreshTokenService(ProductContext dbContext)
        {
            _dbContext = dbContext;
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

            _dbContext.RefreshTokens.Add(refreshToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid tokenId)
        {
            var refreshToken = await _dbContext.RefreshTokens.FindAsync(tokenId);
            if (refreshToken != null)
            {
                _dbContext.Remove(refreshToken);
                await _dbContext.SaveChangesAsync(); 
            }
        }

        public async Task DeleteAllAsync(Guid userId)
        {
            var refreshTokens = await _dbContext.RefreshTokens.Where(x => x.UserId == userId).ToListAsync();
            _dbContext.RemoveRange(refreshTokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken> GetByTokenAsync(string token)
        {
            var refreshToken = await _dbContext.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);

            return refreshToken;
        }
    }
}
