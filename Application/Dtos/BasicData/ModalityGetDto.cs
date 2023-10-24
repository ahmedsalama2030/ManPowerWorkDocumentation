using Core.Common;

namespace Application.Dtos.BasicData;
public class ModalityGetDto:BaseEntityGetTrace
{
  public string Name { get; set; }
    public bool IsActive { get; set; }
}
