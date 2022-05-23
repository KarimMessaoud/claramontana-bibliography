using ClaramontanaBibliography.Data.Entities;
using System;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service.RefreshTokenService
{
    public interface IRefreshTokenService
    {
        Task CreateAsync(RefreshTokenDto refreshTokenDto);
        Task<RefreshToken> GetByTokenAsync(string token);
        Task Delete(Guid tokenId);
    }
}