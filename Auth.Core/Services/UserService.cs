using Auth.Core.DTO;
using Auth.Core.Entities;
using Auth.Core.RepositoryContracts;
using Auth.Core.ServiceContracts;

namespace Auth.Core.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthenticationResponse> Register(RegisterRequest registerRequest)
    {
        if (string.IsNullOrWhiteSpace(registerRequest.Email) ||
            string.IsNullOrWhiteSpace(registerRequest.Password))
        {
            return new AuthenticationResponse { IsSuccessful = false };
        }

        var existingUser = await _userRepository.GetUserByEmail(registerRequest.Email);
        if (existingUser is not null)
            return new AuthenticationResponse { IsSuccessful = false };

        var user = new ApplicationUser
        {
            Email = registerRequest.Email,
            PersonName = registerRequest.PersonName,
            Gender = registerRequest.Gender.ToString(),
            Role = registerRequest.Role.ToString(),
            Password = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password)
        };

        var createdUser = await _userRepository.AddUser(user);
        if (createdUser is null)
            return new AuthenticationResponse { IsSuccessful = false };

        return ToAuthenticationResponse(createdUser);
    }

    public async Task<AuthenticationResponse> Login(LoginRequest loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.Email) ||
            string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return new AuthenticationResponse { IsSuccessful = false };
        }

        var user = await _userRepository.GetUserByEmail(loginRequest.Email);
        if (user is null || string.IsNullOrWhiteSpace(user.Password))
            return new AuthenticationResponse { IsSuccessful = false };

        var isValidPassword = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
        if (!isValidPassword)
            return new AuthenticationResponse { IsSuccessful = false };

        return ToAuthenticationResponse(user);
    }

    private AuthenticationResponse ToAuthenticationResponse(ApplicationUser user) =>
        new()
        {
            UserID = user.UserID,
            Email = user.Email,
            PersonName = user.PersonName,
            Role = user.Role,
            Token = _tokenService.GenerateToken(user),
            IsSuccessful = true
        };
}
