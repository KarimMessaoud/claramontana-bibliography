using ClaramontanaBibliography.Data.Entities;
using ClaramontanaBibliography.Service;
using ClaramontanaBibliography.Service.PasswordHashers;
using ClaramontanaBibliography.Service.RefreshTokenService;
using ClaramontanaBibliography.Service.TokenGenerators;
using ClaramontanaBibliography.Service.TokenValidators;
using ClaramontanaBibliography.WebApi.Authenticators;
using ClaramontanaBibliography.WebApi.Models;
using ClaramontanaBibliography.WebApi.Models.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClaramontanaBibliography.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly Authenticator _authenticator;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthenticationController(IUserService userService, 
                                        IPasswordHasher passwordHasher, 
                                        Authenticator authenticator, 
                                        RefreshTokenValidator refreshTokenValidator, 
                                        IRefreshTokenService refreshTokenService)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _authenticator = authenticator;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage));
                return BadRequest(errorMessages);
            }

            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password does not match confirm password."));
            }

            var existingUserByEmail = await _userService.GetByEmailAsync(registerRequest.Email);
            if(existingUserByEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exists."));
            }

            var existingUserByUsername = await _userService.GetByUsernameAsync(registerRequest.UserName);
            if (existingUserByUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exists."));
            }

            string passwordHash = _passwordHasher.HashPassword(registerRequest.Password);

            var registrationUser = new User
            {
                Id = Guid.NewGuid(),
                Email = registerRequest.Email,
                UserName = registerRequest.UserName,
                PasswordHash = passwordHash
            };

            await _userService.CreateAsync(registrationUser);

            return NoContent();
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage));
                return BadRequest(errorMessages);
            }

            var user = await _userService.GetByUsernameAsync(loginRequest.UserName);

            if (user == null)
            {
                return Unauthorized();
            }

            var isCorrectPassword = _passwordHasher.VerifyPassword(loginRequest.Password, user.PasswordHash);

            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            var response = await _authenticator.AuthenticateAsync(user);

            return Ok(response);
            
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<string> errorMessages = ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage));
                return BadRequest(errorMessages);
            }

            bool isValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            if (!isValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token."));
            }

            var refreshToken = await _refreshTokenService.GetByTokenAsync(refreshRequest.RefreshToken);

            if(refreshToken == null)
            {
                return NotFound(new ErrorResponse("Invalid refresh token."));
            }

            var user = await _userService.GetByIdAsync(refreshToken.UserId);
            if(user == null)
            {
                return NotFound(new ErrorResponse("User not found."));
            }

            var response = await _authenticator.AuthenticateAsync(user);

            return Ok(response);

        }
    }
}
