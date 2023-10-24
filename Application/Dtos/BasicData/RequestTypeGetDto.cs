using Core.Common;

namespace Application.Dtos.BasicData;
public class RequestTypeGetDto : BaseEntityGetTrace
{
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int ModalityId { get; set; }
    public int? ParentId { get; set; }
    public string ParentName { get; set; }
    public string Finding { get; set; }
    public string ModalityName { get; set; }

}
