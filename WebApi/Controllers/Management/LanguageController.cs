using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Common.Pagination;
using Application.Dtos.Localization.Language;
using Application.IBusiness.Localization;
using Core.Interfaces.Common;
namespace WebApi.Controllers.Management;
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class LanguageController : ControllerBase
{
    private readonly ILanguageBusiness _repo;
    private readonly IStringLocalizerCustom _localizer;
    protected readonly ILogCustom _logger;
    public LanguageController(
           ILanguageBusiness LanguageRepositoryBusiness,
         IStringLocalizerCustom localizer,
           ILogCustom logger)
    {
        _repo = LanguageRepositoryBusiness;
        _localizer = localizer;
        _logger = logger;
    }
    [HttpPost]
    // [Authorize(Policy = "permission.language.view")]
    public async Task<IActionResult> Get(PaginationParam paginationParam, CancellationToken cancellationToken)
      => Ok(await _repo.Get(Response, paginationParam));
    [HttpGet("getallList")]
    public async Task<IActionResult> GetallList()
    {
        return Ok(await _repo.GetAllListLanguage());
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
        return Ok(await _repo.GetByIdAsync(id));
     }
    [HttpPost("register")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> Register(LanguageRegisterDto registerDto)
    {
         await _repo.Register(registerDto);
        return NoContent();

    }
    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit(int id, LanguageEditDto entityEdit)
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

    [HttpPut("SetAsDefault/{id}")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> SetAsDefault(int id)
    {
         await _repo.SetAsDefault(id);
        return NoContent();

    }

}

