using Application.Common.Pagination;
using Application.Dtos.Auth.roles;
using Application.Dtos.roles;
using Application.IBusiness.Management;
using Microsoft.AspNetCore.Mvc;
using Users.API.ActionFilter;

namespace Users.API.Controllers.Management;
[Route("api/[controller]")]
//[Authorize(Roles = "sup-admin")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleBusiness _iRoleRepository;
    public RolesController(
    IRoleBusiness IRoleRepository
    )
    {
        _iRoleRepository = IRoleRepository;
    }
    [HttpPost]
    public async Task<IActionResult> Get(PaginationParam paginationParam)
    {
        var result = await _iRoleRepository.Get(Response, paginationParam);
        return Ok(result);
    }
    [HttpGet("getallList")]
    public async Task<IActionResult> GetallList()
    {
        return Ok(await _iRoleRepository.GetAllList());
    }
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _iRoleRepository.GetAll();
        return Ok(result);
    }
    [HttpGet("{id}", Name = "getrole")]
    public async Task<IActionResult> Getrole(int id)
    {
        var result = await _iRoleRepository.GetRole(id);
        if (result.Status)
            return Ok(result.ReturnEntity);
        else
            return BadRequest(result.Message);
    }
    [HttpPost("register")]
     public async Task<IActionResult> Register(RoleRegisterDto RoleRegister)
    {
        await _iRoleRepository.Register(RoleRegister);
        return NoContent();

    }
    [HttpPost("Edit/{id}")]
    [ServiceFilter(typeof(ValidateRoleEditExist))]
    public async Task<IActionResult> Edit(int id, RoleRegisterDto RoleRegister)
    {
        await _iRoleRepository.Edit(id, RoleRegister);
        return NoContent();

    }
    [HttpPost("UpdatePermissions")]
    public async Task<IActionResult> UpdatePermissions(RolePermisionEditDto rolePermisionEditDto)
    {
        await _iRoleRepository.UpdatePermission(rolePermisionEditDto);
        return NoContent();

    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _iRoleRepository.Delete(id);
        return NoContent();

    }
    [HttpPost("deleterange")]
    //  [Authorize(Roles = "sup-admin")]
    public async Task<IActionResult> DeleteRange([FromBody] params int[] roles)
    {
        await _iRoleRepository.DeleteRange(roles);
        return NoContent();

    }
}