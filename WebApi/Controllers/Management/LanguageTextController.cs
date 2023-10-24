 
using Application.Common.Pagination;
using Application.Dtos.Localization.LanguageKey;
using Application.Dtos.Localization.LanguageText;
using Application.IBusiness.Localization;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WebApi.Controllers.Management;
[ApiController]
[Route("api/[controller]")]
// [Authorize(Policy = "LanguageText.view")] 
 [AllowAnonymous]

public class LanguageTextController : ControllerBase
{
    private readonly ILanguageTextBusiness _repo;
    private readonly IStringLocalizerCustom _localizer;
    protected readonly ILogCustom _logger;
    public LanguageTextController(
          ILanguageTextBusiness Repo,
          IStringLocalizerCustom localizer,
          ILogCustom logger)
    {
        _repo = Repo;
        _localizer = localizer;
        _logger = logger;
    }
    [HttpPost]
    // [Authorize(Policy ="Permissions.Language.view")]
    public async Task<IActionResult> Get(LanguageTextParam paginationParam, CancellationToken cancellationToken)
     => Ok(await _repo.Get(Response, paginationParam));
    [HttpGet("getall")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> GetAll([FromQuery] LanguageTextParam paginationParam)
   => Ok(await _repo.GetAll(paginationParam));
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
     return   Ok( await _repo.GetByIdAsync(id));
     }
    [HttpPost("register")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> Register(LanguageTextRegisterDto registerDto)
    {
        await _repo.Register(registerDto);
        return NoContent();
    }
    [HttpPost("Edit/{id}")]
    public async Task<IActionResult> Edit(int id, LanguageTextEditDto entityEdit)
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
   [HttpGet("UpdateToLanguage")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> UpdateToLanguage()
    {
         await _repo.UpdateToLanguage();
        return NoContent();
    }
       [HttpGet("LanguageForLocal")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> GetLanguageForLocal(string lang)
    {
        var result = await _repo.GetLanguageForLocal(lang);
        return  Ok(result);
    }
}