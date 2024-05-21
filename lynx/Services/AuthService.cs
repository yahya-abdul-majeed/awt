using Dapper;
using lynx.Data;
using lynx.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace lynx.Services
{
    public interface IAuthService
    {
        Task<string> Login(Login info);
        Task<string> Register(Register info);
    }
    public class AuthService : IAuthService
    {
        private readonly DapperContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthService(DapperContext context, ITokenService jwtService, IUserService userService)
        {
            _context = context;
            _tokenService = jwtService;
            _userService = userService;
        }
        public async Task<string> Login(Login info)
        {
            var parameters = new
            {
                email = info.email,
                password = info.password
            };

            using(var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(Queries.Login,parameters);
                if(user != null)
                    user.roles = (List<string>)await _userService.GetUserRoles(user.id);
                return _tokenService.GenerateAccessToken(user); //Generate token handles user being null
            }
        }

        public async Task<string> Register(Register info)
        {
            var parameters = new
            {
                username = info.username,
                email = info.email,
                password = info.password
            };

            using(var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(Queries.Register,parameters);
                if(user != null)
                    user.roles = (List<string>)await _userService.GetUserRoles(user.id);
                return _tokenService.GenerateAccessToken(user); //Generate token handles user being null
            }
        }
    }
}
