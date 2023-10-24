 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces.Common;
using Application.IBusiness.Management;
using Application.Dtos.Auth.Audit;

namespace WebApi.Controllers.Management;
[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuditAppController : ControllerBase
{
    private readonly IAuditAppBusiness _repo;
    private readonly IStringLocalizerCustom _localizer;
    protected readonly ILogCustom _logger;
    public AuditAppController(
           IAuditAppBusiness LanguageRepositoryBusiness,
         IStringLocalizerCustom localizer,
           ILogCustom logger)
    {
        _repo = LanguageRepositoryBusiness;
        _localizer = localizer;
        _logger = logger;
    }
    [HttpPost]
    // [Authorize(Policy = "permission.language.view")]
    public async Task<IActionResult> Get(AuditParam paginationParam, CancellationToken cancellationToken)
      => Ok(await _repo.Get(Response, paginationParam));
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      return Ok(await _repo.GetByIdAsync(id));
    }
     [HttpGet("GetTableName")]
    public async Task<IActionResult> GetTableName()
    {
      return Ok(await _repo.GetTableNameAsync());
    }
     [HttpGet("GetState")]
    public async Task<IActionResult> GetState()
    {
      return Ok(await _repo.GetStateAsync());
    }
     [HttpGet("GetLevel")]
    public async Task<IActionResult> GetLevel()
    {
      return Ok(await _repo.GetLevelAsync());
    }
}

