using Application.Common.Pagination;
using Application.Dtos.Auth.ScreenApp;
using Application.IBusiness.Management;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers.Management;
[ApiController]
[Route("api/[controller]")]
// [Authorize(Policy = "ScreenApp.view")] 
public class ScreenAppController : ControllerBase
{
    private readonly IScreenAppBusiness _repo;
    private readonly IStringLocalizerCustom _localizer;
    protected readonly ILogCustom _logger;

    public ScreenAppController(
           IScreenAppBusiness ScreenAppRepositoryBusiness,
         IStringLocalizerCustom localizer,
           ILogCustom logger)
    {
        _repo = ScreenAppRepositoryBusiness;
        _localizer = localizer;
        _logger = logger;
    }
    [HttpPost]
    // [Authorize(Policy ="Permissions.Language.view")]
    public async Task<IActionResult> Get(PaginationParam paginationParam, CancellationToken cancellationToken)
      => Ok(await _repo.Get(Response, paginationParam));
    [HttpGet("getallList")]
    public async Task<IActionResult> GetallList()
    {
        return Ok(await _repo.GetAllList());
    }
    [HttpGet("getall")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParam paginationParam)
    {
        return Ok(await _repo.GetAll(paginationParam));
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
         return Ok ( await _repo.GetByIdAsync(id));
        
    }
    [HttpPost("register")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> Register(ScreenAppRegisterDto registerDto)
    {
        await _repo.Register(registerDto);
        return NoContent();

    }
    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit(int id, ScreenAppEditDto entityEdit)
    {
        await _repo.Edit(id, entityEdit);
        return NoContent();

    }
    [HttpDelete("{id}")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> DeleteById(int id)
    {
        await _repo.DeleteById(id);
        return NoContent();

    }
    [HttpDelete("DeleteSoft/{id}")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> DeleteSoftById(int id)
    {
        await _repo.DeleteSoftById(id);
        return NoContent();

    }
    [HttpPost("DeleteRangeSoft")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]

    public async Task<IActionResult> DeleteRangeSoft([FromBody] params int[] arrayObject)
    {
        await _repo.DeleteRangeSoft(arrayObject);
        return NoContent();

    }
    [HttpPost("DeleteRange")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> DeleteRange([FromBody] params int[] arrayObject)
    {
        await _repo.DeleteRange(arrayObject);
        return NoContent();

    }


}

