using System.Threading.Tasks;
using Application.Dtos.Users;
using Core.Common.Dto;
using Application.Dtos.Message;
using Application.Dtos.Auth.Users;

namespace  Application.IBusiness.Management;
    public interface IAuthBusiness
    {
         Task<RepositoryMessage> Register(UserRegisterDto userRegister);
         Task<RepositoryMessage> RegisterTeam(UserRegisterDto userRegister);
         Task<LoginMessage>  Login(UserLoginDto userLoginDto);
         Task<RepositoryMessage>  UserLock(string id);
         Task<RepositoryMessage>   UserUnLock(string id);
         Task<LoginMessage>  GetPermisionToken();
         Task<LoginMessage> LoginAs(string id);
         Task<LoginMessage> GetToken();


    }
