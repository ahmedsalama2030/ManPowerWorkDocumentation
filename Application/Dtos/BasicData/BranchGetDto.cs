using Core.Common;

namespace Application.Dtos.BasicData;
public class BranchGetDto : BaseEntityGetTrace
{
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
    public string LogoPath { get; set; }
    public string ReportPath { get; set; }
    public string ReportHeaderPath { get; set; }
    public string ReportFooterPath { get; set; }
    public int? DoctorAssignId { get; set; }
    public int UserTenantId { get; set; }
    public string FolderStudyName { get; set; }

}
