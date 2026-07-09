using Auth.Core.Entities;


namespace Auth.Core.ServiceContracts
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user
        /// </summary>
        /// <param name="user">The user to generate a token for</param>
        /// <returns>JWT token string</returns>
        string GenerateToken(ApplicationUser user);
    }
}
