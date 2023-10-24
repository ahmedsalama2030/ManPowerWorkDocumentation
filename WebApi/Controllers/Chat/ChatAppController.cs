using Application.Dtos.ChatApp;
using Application.IBusiness.ChatApp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.Chat;
[ApiController]
[Route("api/[controller]")]
// [AllowAnonymous]
public class ChatAppController : ControllerBase
{
    private readonly IChatBusiness _business;
    public ChatAppController(
          IChatBusiness ChatBusiness
           )
    {
        _business = ChatBusiness;

    }
    [HttpPost]
    public async Task<IActionResult> Get(ChatAppPaginationParam paginationParam, CancellationToken cancellationToken)
      => Ok(await _business.Get(Response, paginationParam));

    [HttpPost("GetChatUnread")]
    public async Task<IActionResult> GetChatUnread(ChatAppPaginationParam paginationParam, CancellationToken cancellationToken)
         => Ok(await _business.GetChatUnread(Response, paginationParam));

    [HttpGet("GetUnreadChatForUser")]
    public async Task<IActionResult> GetUnreadChatForUser(int userId)
         => Ok(await _business.GetUnreadChatForUser(userId));


    [HttpPost("MarkeRead")]
    public async Task<IActionResult> MarkeRead(ChatMarkReadDto chatMarkReadDto)
    {
        await _business.MarkeRead(chatMarkReadDto);
        return NoContent();
    }

    [HttpPost("RegisterOne"),DisableRequestSizeLimit]
    public async Task<IActionResult> RegisterOne([FromForm] ChatRegisterDto chatRegisterDto)
    {
        var result = await _business.RegisterOne(Request, chatRegisterDto);
        return Ok(result);

    }
     [HttpPost("RegisterMore"),DisableRequestSizeLimit]
    public async Task<IActionResult> RegisterMore([FromForm] ChatRegisterMoreDto chatRegisterDto)
    {
        var result = await _business.RegisterMore(Request, chatRegisterDto);
        return Ok(result);

    }
    [HttpGet("GetImage")]
    public IActionResult GetImage(string path)
{
            var result =  _business.GetChatMedia(path);

    return File(result, "image/jpeg");
}


    [HttpDelete("DeleteSoft/{id}")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> DeleteSoftById(int id)
    {
        await _business.DeleteSoftById(id);
        return NoContent();

    }
    [HttpPost("DeleteRangeSoft")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]

    public async Task<IActionResult> DeleteRangeSoft([FromBody] params int[] arrayObject)
    {
        await _business.DeleteRangeSoft(arrayObject);
        return NoContent();

    }
    [HttpPost("DeleteRange")]
    // [Authorize(Roles = "hl-employee,hl-superadmin,hl-admin")]
    public async Task<IActionResult> DeleteRange([FromBody] params int[] arrayObject)
    {
        await _business.DeleteRange(arrayObject);
        return NoContent();

    }


}

