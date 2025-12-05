using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Api.Dtos.Request;
using WeatherForecast.Api.Interfaces.Services;

namespace WeatherForecast.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpRequestDto user)
        {
            try
            {
                var (authResponse, token) = _authService.SignUp(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(24)
                };
                Response.Cookies.Append("auth_token", token, cookieOptions);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "Something went wrong",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequestDto user)
        {
            try
            {
                var (authResponse, token) = _authService.SignIn(user);

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTime.UtcNow.AddHours(24)
                };
                Response.Cookies.Append("auth_token", token, cookieOptions);

                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                return Problem(
                    detail: ex.Message,
                    title: "Something went wrong",
                    statusCode: StatusCodes.Status500InternalServerError
                );
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token");

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
