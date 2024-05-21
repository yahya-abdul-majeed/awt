using Dapper;
using lynx.Data;

namespace lynx.Services
{
    public interface IRoleService
    {
        Task<int> CreateRole(string name);
        Task<int> DeleteRole(int id);
        Task<int> DeleteRole(string name);
        Task<bool> RoleExists(string name);
    }
    public class RoleService : IRoleService
    {
        private readonly DapperContext _context;
        public RoleService(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> CreateRole(string name)
        {
            var parameters = new
            {
                name
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.CreateRole, parameters);
            }
        }

        public async Task<int> DeleteRole(int id)
        {
            var parameters = new
            {
               id 
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.DeleteRoleById, parameters);
            }
        }

        public async Task<int> DeleteRole(string name)
        {
            var parameters = new
            {
               name 
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.DeleteRoleByName, parameters);
            }
        }

        public async Task<bool> RoleExists(string name)
        {
            var parameters = new
            {
               name 
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<bool>(Queries.RoleExists, parameters);
            }
        }
    }
}
