using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos.Message;
using Core.Common;
using Core.Common.Dto;
using global::Application.Common.Pagination;
using Microsoft.AspNetCore.Http;

namespace Application.IBusiness.Common;
public interface IEntitiesBusinessCommon<T, TDtoGet, TRegister, TEdit, TPagination>
 where T : IBaseEntity  
 where TDtoGet : class
 where TRegister : class
where TEdit : class
where TPagination : IPaginationParam
{
    Task<List<TDtoGet>> Get(HttpResponse Response,TPagination paginationParam);
    Task<List<TDtoGet>> GetAll(TPagination paginationParam);
    Task<List<BaseListDto>> GetAllList();
     Task<TDtoGet> GetByIdAsync(int id);
    Task Register(TRegister TRegister);
    void RegisterNoAsync(TRegister TRegister);
    Task<double> GetCount();
    Task  Edit(int id, TEdit entityEdit);
    Task DeleteById(int id);
    Task DeleteSoftById(int id);
    Task DeleteRangeSoft(int[] arrayObjects);
    Task DeleteRange(int[] arrayObjects);
    void Filter(ref IQueryable<T> entities, TPagination paginationParam);
    void Sort(ref IQueryable<T> entities, TPagination paginationParam);
}
