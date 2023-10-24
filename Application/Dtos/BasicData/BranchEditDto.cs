using Microsoft.AspNetCore.Http;

namespace Application.Dtos.BasicData;
public class BranchEditDto 
{
    public int? Id { get; set; }
    public string WhatsAppNumber { get; set; }
    public string ReportName { get; set; }
    public string Phone { get; set; }
    public string AccountManger { get; set; }
    public string AccountMangerContract { get; set; }
    public string FirstHospitalAdmin { get; set; }
    public string FirstHospitalAdminContract { get; set; }
    public string SecondHospitalAdmin { get; set; }
    public string SecondHospitalAdminContract { get; set; }
    public string Address { get; set; }
    public int? DoctorAssignId { get; set; }
    public int UserTenantId { get; set; }
     public IFormFile  LogoPathFile { get; set; }
    public IFormFile  ReportPathFile { get; set; }
    public IFormFile HeaderPathFile{ get; set; }
    public IFormFile FooterPathFile{ get; set; }
    public bool IsDeletedLogoPathFile { get; set; }
    public bool IsDeletedReportPathFile { get; set; }
    public bool IsDeletedHeaderPathFile { get; set; }
    public bool IsDeletedFooterPathFile { get; set; }
    public string FolderStudyName { get; set; }


}
