using Core.Common;
namespace Application.Dtos.Auth.ModuleApp;
public class ModuleAppEditDto : ModuleAppRegisterDto,IBaseId
{
 public int Id { get; set; }
}
