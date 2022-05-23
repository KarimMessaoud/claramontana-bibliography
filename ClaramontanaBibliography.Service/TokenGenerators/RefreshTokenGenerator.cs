using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service.TokenGenerators
{
    public class RefreshTokenGenerator
    {
        private readonly TokenGenerator _tokenGenerator;
        private readonly AuthenticationConfiguration _configuration;

        public RefreshTokenGenerator(TokenGenerator tokenGenerator, AuthenticationConfiguration configuration)
        {
            _tokenGenerator = tokenGenerator;
            _configuration = configuration;
        }

        public string  GenerateToken()
        {
            return _tokenGenerator.GenerateToken(_configuration.RefreshTokenSecretKey,
                 _configuration.Issuer,
                 _configuration.Audience,
                 _configuration.RefreshTokenExpirationMinutes);
        }
    }
}
