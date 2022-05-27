using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.Service.RefreshTokenService
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
    }
}
