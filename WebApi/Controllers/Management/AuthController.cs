using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.IBusiness.Management;
using Application.Dtos.Users;
using Application.Dtos.Auth.Users;

namespace Users.API.Controllers.Management
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthBusiness _iAuthRepository;
        public AuthController(
            IAuthBusiness IAuthRepository
            )
        {
            _iAuthRepository = IAuthRepository;
        }

        [HttpPost("register")]
        //    [Authorize(Roles = "sup-admin")]
        public async Task<IActionResult> Register(UserRegisterDto userRegister)
        {
            var result = await _iAuthRepository.Register(userRegister);
            if (result.Status)
                return NoContent();
            else
                return BadRequest(result.Message);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var result = await _iAuthRepository.Login(userLoginDto);
            if (result.Status)
                return Ok(result.ReturnEntity);
            else if (result.StatusCode == 401)
                return Unauthorized(result.Message);
            else
                return BadRequest(result.Message);
        }
        [HttpGet("userLock/{id}")]
        public async Task<IActionResult> UserLock(string id)
        {
            var result = await _iAuthRepository.UserLock(id);
            if (result.Status)
                return NoContent();
            else
                return BadRequest(result.Message);
        }
        [HttpPost("LoginAs/{userId}")]
        public async Task<IActionResult> LoginAs(string userId)
        {
            var result = await _iAuthRepository.LoginAs(userId);
            if (result.Status)
                return Ok(result.ReturnEntity);
            else if (result.StatusCode == 401)
                return Unauthorized(result.Message);
            else
                return BadRequest(result.Message);
        }
          [HttpGet("GetToken")]
        public async Task<IActionResult> GetToken()
        {
            var result = await _iAuthRepository.GetToken();
            if (result.Status)
                return Ok(result.ReturnEntity);
            else if (result.StatusCode == 401)
                return Unauthorized(result.Message);
            else
                return BadRequest(result.Message);
        }
        [HttpGet("PermisionToken")]
        public async Task<IActionResult> PermisionToken()
        {
            var result = await _iAuthRepository.GetPermisionToken();
            if (result.Status)
                return Ok(result.ReturnEntity);

            else
                return BadRequest(result.Message);
        }
        [HttpGet("userUnLock/{id}")]
        public async Task<IActionResult> UserUnLock(string id)
        {
            var result = await _iAuthRepository.UserUnLock(id);
            if (result.Status)
                return NoContent();
            else
                return BadRequest(result.Message);

        }

    }
}
