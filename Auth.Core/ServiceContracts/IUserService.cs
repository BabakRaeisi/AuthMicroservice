using Auth.Core.DTO;

namespace Auth.Core.ServiceContracts
{
    public interface IUserService
    {
        Task<AuthenticationResponse> Register(RegisterRequest registerRequest);
        Task<AuthenticationResponse> Login(LoginRequest loginRequest);
    }
}

