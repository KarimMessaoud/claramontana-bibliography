using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.Service
{
    public class AuthenticationConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AccessTokenSecretKey { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
        public string RefreshTokenSecretKey { get; set; }
    }
}
