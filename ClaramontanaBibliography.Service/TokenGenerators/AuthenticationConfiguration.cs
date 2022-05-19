﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.Service.TokenGenerators
{
    public class AuthenticationConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AccessTokenSecretKey { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
    }
}
