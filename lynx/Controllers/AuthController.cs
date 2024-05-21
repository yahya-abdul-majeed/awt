using lynx.Models.Auth;
using lynx.Services;
using Microsoft.AspNetCore.Mvc;

namespace lynx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<string>> Login([FromBody] Login info)
        {
            try
            {
                var token = await _authService.Login(info);
                if(string.IsNullOrEmpty(token))
                    return NotFound("Incorrect Login details");
                return Ok(token);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "Something went wrong!");
            }
        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<string>> Register([FromBody] Register info)
        {
            try
            {
                var token = await _authService.Register(info);
                if (string.IsNullOrEmpty(token))
                    return BadRequest("Registration failed");
                return Ok(token);
            }
            catch(InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch(Exception)
            {
                return StatusCode(500, "Something went wrong!");
            } 
        }
        
        //public async Task<ActionResult<string>> RefreshToken(){}
    }
}
