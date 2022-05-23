using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataServer.API.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [HttpGet("data")]
        public IActionResult Index()
        {
            var id = HttpContext.User.FindFirstValue("id");
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            var username = HttpContext.User.FindFirstValue(ClaimTypes.Name);
            return Ok();
        }
    }
}
