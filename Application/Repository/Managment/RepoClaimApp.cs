using Application.IRpository.Managment;
using Core.Entities.Management;
using Core.Interfaces.Common;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
namespace Application.Repository.Managment;
public class RepoClaimApp : RepositoryApp<ClaimApp>, IRepoClaimApp
{
    public RepoClaimApp(
              AppDbContext _db,
              ILogCustom  ILogCustom
          ) : base(_db,ILogCustom)
    { }
    
}

