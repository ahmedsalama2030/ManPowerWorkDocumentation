using Application.Dtos.Auth.ScreenApp;
using Core.Entities.Management;
using Core.Interfaces;
using Core.Interfaces.Common;
using FluentValidation;
namespace Application.Validations.Auth;
public class ScreenAppValidator : AbstractValidator<ScreenAppRegisterDto>
{
    private readonly IRepositoryApp<ScreenApp> _repo;
    private readonly IStringLocalizerCustom _localizer;

    public ScreenAppValidator(
    IRepositoryApp<ScreenApp> Repo,
    IStringLocalizerCustom localizer
    )
    {
        _repo = Repo;
       _localizer = localizer;
        RuleFor(x => x.NameAr)  
           .CustomAsync(async (hospital, context, cancellation) =>
        {
            var existingHospital = await _repo.SingleOrDefaultAsNoTrackingAsync(a=>a.NameAr==hospital);

            if (existingHospital != null)
            {
                context.AddFailure(_localizer["namefound"]);
            }
        });
        RuleFor(x => x.NameEn)  
           .CustomAsync(async (hospital, context, cancellation) =>
        {
            var existingHospital = await _repo.SingleOrDefaultAsNoTrackingAsync(a=>a.NameEn==hospital);

            if (existingHospital != null)
            {
                context.AddFailure(_localizer["namefound"]);
            }
        });
    }

   
}
