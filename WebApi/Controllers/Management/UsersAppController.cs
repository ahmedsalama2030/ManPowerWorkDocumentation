using Application.Common.Pagination;
using Application.Dtos.Auth.roles;
using Application.Dtos.Auth.Users;
using Application.Dtos.Users;
using Application.IBusiness.Management;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
namespace Users.API.Controllers.Management;
[Route("api/[controller]")]
[ApiController]
//  [Authorize(Roles = "sup-admin,ga-email,ga-message")]
public class UsersAppController : ControllerBase
{
    private readonly IUserBusiness _iUserRepository;
    private readonly IStringLocalizerCustom _localizer;

    public UsersAppController(
     IUserBusiness IUserRepository,
      IStringLocalizerCustom localizer
      )
    {
        _iUserRepository = IUserRepository;
        _localizer = localizer;
    }
    [HttpPost]
    //  [Authorize(Roles = "sup-admin")]

    public async Task<IActionResult> Get(UserParam paginationParam)
    {
        var result = await _iUserRepository.Get(Response, paginationParam);
        return Ok(result);
    }


    [HttpGet("{userId}/getUsersForRole/{role}")]
    //    [Authorize(Roles = "sup-admin,ga-email,ga-message")]

    public async Task<IActionResult> GetUsersForRole(int userId, string role, [FromQuery] UserParam paginationParam)
    {
        if (string.IsNullOrEmpty(role))
            return BadRequest(_localizer["notfound"].Value);
        var result = await _iUserRepository.GetUsersForRole(Response, userId, role, paginationParam);
        return Ok(result);
    }
    //      [Authorize(Roles = "sup-admin,user-admin")]

    [HttpGet("rolesuser/{id}")]
    // [Authorize(Roles = "sup-admin")]

    public async Task<IActionResult> GetRoleUser(int id)
    {
        var result = await _iUserRepository.GetRoleUser(id);
        return Ok(result);
    }
    [HttpGet("GetAllList")]

    public async Task<IActionResult> GetAllList()
    {
        var result = await _iUserRepository.GetAllList();
        return Ok(result);
    }
    [HttpGet("GetUsersAnalysis")]
    public async Task<IActionResult> GetUsersAnalysis()
    {
        var result = await _iUserRepository.UsersAnalysis();
        return Ok(result);
    }
    [HttpGet("{id}")]
    // [Authorize(Roles = "sup-admin")]

    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _iUserRepository.GetUser(id);
        if (result.Status)
            return Ok(result.ReturnEntity);
        else
            return BadRequest(result.Message);
    }

    [HttpPost("assignroles/{userId}")]
    //   [Authorize(Roles = "sup-admin")]
    public async Task<IActionResult> AssignRoles(int userId, params string[] roles)
    {
        await _iUserRepository.AssignRoles(userId, roles);
        return NoContent();

    }

    [HttpPost("changepassword/{id}")]
    //  [Authorize(Roles = "sup-admin,ga-changepassword")]

    public async Task<IActionResult> ChangePassword(int id, PaaswordDto newpassword)
    {
        await _iUserRepository.ChangePassword(id, newpassword);
        return NoContent();

    }
    [HttpDelete("{id}")]
    //  [Authorize(Roles = "sup-admin")]

    public async Task<IActionResult> DeleteById(int id)
    {
        await _iUserRepository.DeleteById(id);
        return NoContent();

    }


    [HttpPost("Edit/{id}")]
    //[Authorize(Roles = "sup-admin")]
    public async Task<IActionResult> Edit(int id, UserEditDto userEdit)
    {
        await _iUserRepository.Edit(id, userEdit);
        return NoContent();

    }
    [HttpPost("deleterange")]
    // [Authorize(Roles = "sup-admin")]

    public async Task<IActionResult> DeleteRange([FromBody] params int[] users)
    {
        await _iUserRepository.DeleteHard(users);
        return NoContent();
    }
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("deletehard")]
    //   [Authorize(Roles = "sup-admin")]
    public async Task<IActionResult> deleteHard([FromBody] params int[] users)
    {
        await _iUserRepository.DeleteHard(users);
        return NoContent();

    }
    [HttpPost("UpdatePermissions")]
    public async Task<IActionResult> UpdatePermissions(UserPermisionEditDto userPermisionEditDto)
    {
        await _iUserRepository.UpdatePermission(userPermisionEditDto);
        return NoContent();
    }
    [HttpPost("ChangeCurrentTime")]
    public async Task<IActionResult> ChangeCurrentTime(ChangeCurrentTimeDto userPermisionEditDto)
    {
        await _iUserRepository.ChangeCurrentTime(userPermisionEditDto);
        return NoContent();
    }
    
}

