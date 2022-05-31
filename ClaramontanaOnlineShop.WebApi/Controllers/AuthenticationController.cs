using ClaramontanaOnlineShop.Data.Entities;
using ClaramontanaOnlineShop.Service.RefreshTokenService;
using ClaramontanaOnlineShop.Service.TokenValidators;
using ClaramontanaOnlineShop.WebApi.Authenticators;
using ClaramontanaOnlineShop.WebApi.Models;
using ClaramontanaOnlineShop.WebApi.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ClaramontanaOnlineShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticationController(UserManager<User> userManager,
                                        Authenticator authenticator, 
                                        RefreshTokenValidator refreshTokenValidator, 
                                        IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = registerRequest.Email,
                    UserName = registerRequest.UserName,
                };

                IdentityResult result = await _userManager.CreateAsync(user, registerRequest.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest(result.Errors.Select(x => x.Description));
            }

            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage));
            return BadRequest(errorMessages);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(loginRequest.UserName);

                if (user == null)
                {
                    return Unauthorized();
                }

                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (!isCorrectPassword)
                {
                    return Unauthorized();
                }

                var response = await _authenticator.AuthenticateAsync(user);

                return Ok(response);
            }

            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage));
            return BadRequest(errorMessages);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (ModelState.IsValid)
            {
                bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

                if (!isValidRefreshToken)
                {
                    return BadRequest(new ErrorResponse("Invalid refresh token."));
                }

                var refreshToken = await _refreshTokenService.GetByTokenAsync(refreshRequest.RefreshToken);

                if (refreshToken == null)
                {
                    return NotFound(new ErrorResponse("Invalid refresh token."));
                }

                await _refreshTokenService.DeleteAsync(refreshToken.Id);

                var user = await _userManager.FindByIdAsync(refreshToken.UserId.ToString());
                if (user == null)
                {
                    return NotFound(new ErrorResponse("User not found."));
                }

                var response = await _authenticator.AuthenticateAsync(user);

                return Ok(response);
            }

            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage));
            return BadRequest(errorMessages);
        }

        [Authorize]
        [HttpDelete("logout")]
        public async Task<IActionResult> Logout()
        {
            string rawUserId = HttpContext.User.FindFirstValue("id");

            if(!Guid.TryParse(rawUserId, out Guid userId))
            {
                return Unauthorized();
            }

            await _refreshTokenService.DeleteAllAsync(userId);

            return NoContent();
        }
    }
}
