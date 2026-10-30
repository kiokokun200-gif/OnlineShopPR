
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.API.Services;
using OnlineShop.Core.DTOs.Auth;

namespace OnlineShop.API.Controller
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {

            _authService = authService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync(RegisterRequest registerRequest)
        {
            if (!registerRequest.password.Equals(registerRequest.passwordConfirm))
            {
                return BadRequest(
                    new RegisterResponse
                    {
                        message = "Пароли не совпадают",
                        isRegister = false
                    }
                    );
            }

            var (message, isReg) = await _authService.RegisterAsync(registerRequest.login , registerRequest.password);
            if (!isReg)
            {
                return BadRequest(
                    new RegisterResponse
                    {
                        message = message,
                        isRegister = false
                    }
                );
            }
            return Ok(
                    new RegisterResponse
                    {
                        message = message,
                        isRegister = true
                    }
                );
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(LoginRequest loginRequest)
        {

            var (message, token, user) = await _authService.LoginAsync(loginRequest.login, loginRequest.password);
            if (token == null)
            {
                return BadRequest(new
                LoginResponse{
                    message = message,
                    isLogin = true,
                    token = token,
                    user = user
                });
            }
            return Ok(
                    new
                    LoginResponse
                    {
                        message = message,
                        isLogin = true,
                        token = token,
                        user = user
                    }
                );

        }

    }
}
