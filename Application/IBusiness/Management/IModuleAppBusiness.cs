using Application.Common.Pagination;
using Application.Dtos.Auth.ModuleApp;
using Application.IBusiness.Common;
using Core.Entities.Management;
namespace Application.IBusiness.Management;
public interface IModuleAppBusiness: IEntitiesBusinessCommon<
 ModuleApp,
ModuleAppGetDto,
ModuleAppRegisterDto,
ModuleAppEditDto,
PaginationParam>
 {}
