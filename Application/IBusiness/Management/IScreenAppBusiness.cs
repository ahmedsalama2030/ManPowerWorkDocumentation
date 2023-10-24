using Application.Common.Pagination;
using Application.Dtos.Auth.ScreenApp;
using Application.IBusiness.Common;
using Core.Entities.Management;

namespace Application.IBusiness.Management;
public interface IScreenAppBusiness: IEntitiesBusinessCommon<
 ScreenApp,
ScreenAppGetDto,
ScreenAppRegisterDto,
ScreenAppEditDto,
PaginationParam>
 {}
