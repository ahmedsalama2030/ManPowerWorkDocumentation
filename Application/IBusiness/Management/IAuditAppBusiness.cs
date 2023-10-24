using System;
using Application.Common.Pagination;
using Application.Dtos.Auth.Audit;
using Core.Entities.Management;
using Microsoft.AspNetCore.Http;

namespace Application.IBusiness.Management;
public interface IAuditAppBusiness
{
    Task<List<AuditGetDto>> Get(HttpResponse Response, AuditParam paginationParam);
    Task<Audit> GetByIdAsync(int id);
    Task<List<AuditList>> GetTableNameAsync();
    Task<List<AuditList>> GetStateAsync();
    Task<List<AuditList>> GetLevelAsync();
}

