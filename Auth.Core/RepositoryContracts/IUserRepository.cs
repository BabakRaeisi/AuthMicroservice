using Auth.Core.Entities;

namespace Auth.Core.RepositoryContracts
{
    public interface IUserRepository
    {
        /// <summary>  
        /// Method to add a user to the data store and return added user
        /// 
        /// </summary>
        ///  /// <param name="applicationUser">The user to be added.</param>
        ///  <returns>  </returns>

        Task<ApplicationUser?> AddUser(ApplicationUser user);
        /// <summary>  
        /// to get a user by email and password from the data store
        /// 
        /// </summary>
        /// <param name="email">The email of the user.</param>
        /// <pram name="password">The password of the user.</param></pram>
        ///  <returns>  </returns>
        Task<ApplicationUser?> GetUserByEmail(string? email);
    }
}
