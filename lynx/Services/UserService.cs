using Dapper;
using lynx.Data;
using lynx.Models.Auth;

namespace lynx.Services
{
    public interface IUserService
    {
        Task<bool> UserExists(string email);
        Task<IEnumerable<string>> GetUserRoles(int userid);
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<User>> GetUsersForGroup(int group_id);
    }
    public class UserService : IUserService
    {
        private readonly DapperContext _context;
        public UserService(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<string>> GetUserRoles(int userid)
        {
            var parameters = new {userid};
            using (var connection = _context.CreateConnection())
            {
                var roles = await connection.QueryAsync<string>(Queries.GetUserRoles, parameters);
                return roles;
            };
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<User>(Queries.GetUsers);
            };
        }

        public async Task<IEnumerable<User>> GetUsersForGroup(int group_id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<User>(Queries.GetUsersForGroup,new {group_id});
            };
        }

        public Task<bool> UserExists(string email)
        {
            throw new NotImplementedException();
        }
    }
}
