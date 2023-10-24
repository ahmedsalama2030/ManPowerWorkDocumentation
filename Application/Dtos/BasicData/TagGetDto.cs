using Core.Common;

namespace Application.Dtos.BasicData;
public class TagGetDto : BaseEntityGetTrace
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
}
