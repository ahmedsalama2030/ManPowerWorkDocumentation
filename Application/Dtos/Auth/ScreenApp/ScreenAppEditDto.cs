namespace Application.Dtos.Auth.ScreenApp;
using Core.Common;
public class ScreenAppEditDto:ScreenAppRegisterDto,IBaseId
{
    public int Id { get; set; }
}
