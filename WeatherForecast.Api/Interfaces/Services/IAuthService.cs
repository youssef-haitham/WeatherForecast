using WeatherForecast.Api.Dtos.Request;
using WeatherForecast.Api.Dtos.Response;

namespace WeatherForecast.Api.Interfaces.Services
{
    public interface IAuthService
    {
        (AuthResponseDto authResponse, string token) SignUp(SignUpRequestDto user);
        (AuthResponseDto authResponse, string token) SignIn(SignInRequestDto user);
    }
}
