using Dapper;
using Auth.Core.Entities;
using Auth.Core.RepositoryContracts;
using Auth.Infrastructure.DbContext;

namespace Auth.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        public readonly DapperDbContext _dbContext;

        public UserRepository(DapperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ApplicationUser?> AddUser(ApplicationUser user)
        {
            user.UserID = Guid.NewGuid();

            const string query = """
                INSERT INTO public."Users" ("UserID", "Email", "PersonName", "Gender", "Password")
                VALUES (@UserID, @Email, @PersonName, @Gender, @Password);
                """;

            int rowCountAffected = await _dbContext.DbConnection.ExecuteAsync(query, user);
            return rowCountAffected > 0 ? user : null;
        }

        public async Task<ApplicationUser?> GetUserByEmail(string? email)
        {
            const string query = """
                SELECT "UserID", "Email", "PersonName", "Gender", "Password"
                FROM public."Users"
                WHERE "Email" = @Email;
                """;

            return await _dbContext.DbConnection.QueryFirstOrDefaultAsync<ApplicationUser>(
                query,
                new { Email = email }
            );
        }
    }
}
