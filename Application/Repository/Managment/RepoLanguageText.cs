using Application.IRpository.Managment;
using AutoMapper;
using Core.Entities.Management;
using Core.Interfaces.Common;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.Managment;
public class RepoLanguageText : RepositoryApp<LanguageText>, IRepoLanguageText
{
    protected readonly IMapper _mapper;

    public RepoLanguageText(
              AppDbContext _db,
              ILogCustom ILogCustom,
IMapper mapper
          ) : base(_db, ILogCustom)
    {
        _mapper = mapper;
    }
//     public async Task<List<LanguageKeysView>> GetLanguageTextLocalSPDto(string lang)
//     {
//         return await _db.Set<LanguageKeysView>().FromSqlRaw($"[Management].[GetLanguageKeys] @language = '{lang}'").ToListAsync();
//    }


}

