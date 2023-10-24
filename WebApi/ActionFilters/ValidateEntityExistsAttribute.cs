using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Common;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using WebApi.Resources;

namespace WebApi.ActionFilters
{
    public class ValidateEntityExistsAttribute<T> : IAsyncActionFilter where T : BaseId
    {
        private readonly IRepositoryApp<T> _department;
        private readonly IStringLocalizer<Resource> _localizer;
        public ValidateEntityExistsAttribute(
             IRepositoryApp<T> Department,
           IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
            _department = Department;

        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Guid id = Guid.Empty;
            id = (Guid)context.ActionArguments["id"];
            var entity = await _department.SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (entity == null)
            {
                context.Result = new NotFoundObjectResult(_localizer["notfound"].Value);
                return;
            }
            else
                context.HttpContext.Items.Add("entity", entity);
            await next();
        }
    }
}




