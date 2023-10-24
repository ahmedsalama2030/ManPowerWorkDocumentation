namespace Application.Dtos.BasicData;
public class RequestTypeEditDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public int ModalityId { get; set; }
    public int? ParentId { get; set; }
    public string Finding { get; set; }
}
