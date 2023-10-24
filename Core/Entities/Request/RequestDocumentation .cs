using System;
using Core.Common;
using Core.Entities.Management;
using Core.Enums;
namespace Core.Entities.Request;
public class RequestDocumentation : BaseEntity
{
public int UserId { get; set; }
public long RequestNumber { get; set; }
public string CompanyEstablishmentContractPath { get; set; }
public string RiskReportPath { get; set; }
public string OwnershipContractCenterPath { get; set; }
public string TaxCardPath { get; set; }
public string ActivitiesAndTrainingProgramsPath { get; set; }
public string VocationalTrainingActivityPath { get; set; }
public string MembersCompanyPath { get; set; }
public string PaymentReceiptPath { get; set; }
public string EngineeringDrawingCenterPath { get; set; }
public string PresentationCurriculaPath { get; set; }
public string CivilDefenseReportPath { get; set; }
public string StatementCoachesPath { get; set; }
public string EmergencyPlanReportPath { get; set; }
public string StatementAdministratorsAndInsuranceFormsPath { get; set; }
public RequestStatus RequestStatus { get; set; }

public User User { get; set; }
}

