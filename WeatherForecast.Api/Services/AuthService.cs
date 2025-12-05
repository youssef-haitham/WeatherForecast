using WeatherForecast.Api.Dtos.Request;
using WeatherForecast.Api.Dtos.Response;
using WeatherForecast.Api.Entities;
using WeatherForecast.Api.Interfaces.Providers;
using WeatherForecast.Api.Interfaces.Repositories;
using WeatherForecast.Api.Interfaces.Services;

namespace WeatherForecast.Api.Services
{
    public class AuthService(IUserRepository userRepo, ITokenProvider tokenProvider, IHashProvider hashProvider) : IAuthService
    {
        private readonly IUserRepository _userRepo = userRepo;
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IHashProvider _hashProvider = hashProvider;

        public (AuthResponseDto authResponse, string token) SignUp(SignUpRequestDto user)
        {
            var hashedPassword = _hashProvider.HashPassword(user.Password);

            if (_userRepo.UserExistsByEmail(user.Email))
            {
                throw new Exception("Email already exists");
            }
            var newuser = new User()
            {
                Email = user.Email,
                Name = user.Name,
                PasswordHash = hashedPassword
            };
            var addedUser = _userRepo.AddUser(newuser);
            var token = _tokenProvider.CreateToken(addedUser.Id, addedUser.Email);

            return (new AuthResponseDto() { Id = addedUser.Id, Email = addedUser.Email, Name = addedUser.Name }, token);
        }

        public (AuthResponseDto authResponse, string token) SignIn(SignInRequestDto user)
        {
            var userExist = _userRepo.GetUserByEmail(user.Email) ?? throw new BadHttpRequestException("Email or Password are incorrect");

            var passwordVerified = _hashProvider.Verify(user.Password, userExist.PasswordHash);
            if (!passwordVerified) throw new BadHttpRequestException("Email or Password are incorrect");

            var token = _tokenProvider.CreateToken(userExist.Id, userExist.Email);
            return (new AuthResponseDto() { Id = userExist.Id, Email = userExist.Email, Name = userExist.Name }, token);
        }
    }
}