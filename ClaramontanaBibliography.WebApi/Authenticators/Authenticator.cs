using ClaramontanaBibliography.Data.Entities;
using ClaramontanaBibliography.Service.RefreshTokenService;
using ClaramontanaBibliography.Service.TokenGenerators;
using ClaramontanaBibliography.WebApi.Models.Responses;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.WebApi.Authenticators
{
    public class Authenticator
    {
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;
        private readonly IRefreshTokenService _refreshTokenService;

        public Authenticator(AccessTokenGenerator accessTokenGenerator,
                             RefreshTokenGenerator refreshTokenGenerator, 
                             IRefreshTokenService refreshTokenService)
        {
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthenticatedUserResponse> AuthenticateAsync(User user)
        {
            var accessToken = _accessTokenGenerator.GenerateToken(user);
            var refreshToken = _refreshTokenGenerator.GenerateToken();

            var refreshTokenDto = new RefreshTokenDto
            {
                Token = refreshToken,
                UserId = user.Id
            };

            //Store refresh token in the database
            await _refreshTokenService.CreateAsync(refreshTokenDto);

            return new AuthenticatedUserResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
