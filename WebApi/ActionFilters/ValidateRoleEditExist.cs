using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using Core.Entities.Management;
using WebApi.Resources;
using Application.Dtos.roles;

namespace Users.API.ActionFilter
{
    public class ValidateRoleEditExist : IAsyncActionFilter
    {
        private readonly IRepositoryApp<Role> _roleRepo;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Resource> _localizer;
        public ValidateRoleEditExist(
        IRepositoryApp<Role> roleRepo,
          IMapper mapper,
        IStringLocalizer<Resource> localizer)
        {
            _localizer = localizer;
            _roleRepo = roleRepo;
            _mapper = mapper;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            int id = 0;
            id =  (int)context.ActionArguments["id"];
            var roleOld = await _roleRepo.SingleOrDefaultAsync(a => a.Id == id && a.IsDeleted == false);
            if (roleOld == null)
            {
                context.Result = new BadRequestObjectResult(_localizer["notfound"].Value);
                return;
            }
            var role = context.ActionArguments.Values.ToArray()[1] as RoleRegisterDto;
            if (roleOld.Name != role.Name || roleOld.NameAr != role.NameAr || roleOld.NameEn != role.NameEn)
            {
                var entity = (await _roleRepo.GetAllAsync(x => x.Name == role.Name || x.NameEn == role.NameEn && x.NameAr == role.NameAr));
                if (entity.Count() > 1)
                {
                    context.Result = new BadRequestObjectResult(_localizer["rolefound"].Value);
                    return;
                }
            }
            context.HttpContext.Items.Add("entity", roleOld);
            await next();
        }


    }
}




