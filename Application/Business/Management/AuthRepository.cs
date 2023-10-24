using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dtos.Auth.Users;
using Application.Dtos.Message;
using Application.Dtos.Users;
using Application.IBusiness.Common;
using Application.IBusiness.Management;
using Application.Services;
using AutoMapper;
using Core.Dtos.Settings;
using Core.Entities.Management;
using Core.Enums;
using Core.Exceptions;
using Core.Interfaces;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
namespace Infrastructure.Business.Management;
public class AuthBusiness : IAuthBusiness
{
    #region  init
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    public readonly IRepositoryApp<UserRole> _UserRole;
    public readonly IRepositoryApp<User> _userRepo;
    public readonly IRepositoryApp<RoleClaim> _roleClaim;
    private readonly IMapper _mapper;
    private readonly SignInManager<User> _signInManager;
    private readonly IRepositoryMessage _iRepositoryMessage;
    private readonly IConfiguration _config;
    private readonly IStringLocalizerCustom _localizer;
    private readonly MaxTimeToken _timeToken;
    private readonly ILogCustom _logger;
    protected readonly IClockService _iClockService;
    private readonly IHttpContextAccessor _accssor;
    private readonly IChatAppConnection _iChatAppConnection;

    public AuthBusiness(
           IStringLocalizerCustom localizer,
           IOptions<MaxTimeToken> TimeToken,
            UserManager<User> User,
            IConfiguration config,
            IMapper mapper,
            SignInManager<User> signInManager,
            IRepositoryMessage IRepositoryMessage,
            ILogCustom logger,
            RoleManager<Role> roleManager,
           IRepositoryApp<UserRole> UserRole,
           IRepositoryApp<RoleClaim> RoleClaim,
           IClockService iClockService,
     IHttpContextAccessor Accssor,
     IChatAppConnection IChatAppConnection,
     IRepositoryApp<User> userRepo
     )
    {

        _config = config;
        _signInManager = signInManager;
        _iRepositoryMessage = IRepositoryMessage;
        _mapper = mapper;
        _userManager = User;
        _localizer = localizer;
        _timeToken = TimeToken.Value;
        _logger = logger;
        _roleManager = roleManager;
        _UserRole = UserRole;
        _iClockService = iClockService;
        _accssor = Accssor;
        _iChatAppConnection = IChatAppConnection;
        _roleClaim = RoleClaim;
        _userRepo = userRepo;

    }
    #endregion
    public async Task<LoginMessage> Login(UserLoginDto userLoginDto)
    {
        var user = await _userManager.FindByEmailAsync(userLoginDto.Email) ?? throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var result = await _signInManager.CheckPasswordSignInAsync(user, userLoginDto.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogInFail(user.UserName, userLoginDto.Password);
            throw new ExceptionCommonReponse(MessageReturn.Mangement_FailLogin, 400);
        }
        user.LastLogin = _iClockService.Now;
        await _userManager.UpdateAsync(user);
        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == userLoginDto.Email);
        var users = _mapper.Map<UserListDto>(appUser);
        var token = await RoleJWTToken(appUser);
        // var tokenPermision = await PermisionJWTToken(appUser);
        _logger.LogInSuccess(user.UserName);
        _iChatAppConnection.RemoveUser(users.Id);
        return SuccessMessage(token, "", users);
    }
    public async Task<LoginMessage> GetPermisionToken()
    {
        var userName = _accssor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        var user = await _userManager.FindByNameAsync(userName);
        var tokenPermision = await PermisionJWTToken(user);
        return SuccessMessage("", tokenPermision, null);

    }
    public async Task<RepositoryMessage> Register(UserRegisterDto userRegister)
    {
        var userToCreate = _mapper.Map<User>(userRegister);
        var userEmail = await _userManager.FindByEmailAsync(userRegister.Email);
        if (userEmail != null)
            throw new ExceptionCommonReponse(MessageReturn.Mangement_EmailFound, 400);
        LogRowRegister(ref userToCreate);
        userToCreate.UserName = userRegister.Email;
        var result = await _userManager.CreateAsync(userToCreate, userRegister.Password);
        if (result.Succeeded)
        {
            _logger.SignUpSuccess(userRegister.Email);
            var user = await _userManager.FindByIdAsync(userToCreate.Id.ToString());
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles.Any())
                await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!string.IsNullOrEmpty(userRegister.RoleName))
                await _userManager.AddToRoleAsync(user, userRegister.RoleName);
            return _iRepositoryMessage.SuccessMessage();
        }
        else
            throw new ExceptionCommonReponse(MessageReturn.Common_FailRegister, 400);
    }

    public async Task<RepositoryMessage> RegisterTeam(UserRegisterDto userRegister)
    {
        var userId = int.Parse(_accssor.HttpContext.User.FindFirstValue("TenantId"));

        var userToCreate = _mapper.Map<User>(userRegister);
        var userEmail = await _userManager.FindByEmailAsync(userRegister.Email);
        if (userEmail != null)
            throw new ExceptionCommonReponse(MessageReturn.Mangement_EmailFound, 400);
        userToCreate.UserTenantId = userId;
        userToCreate.UserName = userRegister.Email;
        LogRowRegister(ref userToCreate);
        userToCreate.ClientId = userToCreate.ClientId ?? Guid.NewGuid();
        var result = await _userManager.CreateAsync(userToCreate, userRegister.Password);
        if (!result.Succeeded)
            throw new ExceptionCommonReponse(MessageReturn.Common_FailRegister, 400);
        var userToReturn = _mapper.Map<UserListDto>(userToCreate);
        _logger.Info<User>(MessageReturn.Common_SuccessRegister, userRegister, AuditType.register);
        return _iRepositoryMessage.SuccessMessage(userToReturn);

    }
    public async Task<RepositoryMessage> UserLock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var result = await _userManager.SetLockoutEnabledAsync(user, true);
        var resultDate = await _userManager.SetLockoutEndDateAsync(user, _iClockService.Now.AddYears(10));
        if (!result.Succeeded && !resultDate.Succeeded)
            throw new ExceptionCommonReponse(MessageReturn.Mangement_FailUserLock, 400);
        _logger.Info<User>("successUserLock", id, AuditType.edit);
        return _iRepositoryMessage.SuccessMessage();

    }
    public async Task<RepositoryMessage> UserUnLock(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        user.LockoutEnabled = true;
        var resultDate = await _userManager.SetLockoutEndDateAsync(user, _iClockService.Now - TimeSpan.FromMinutes(1));
        var result = await _userManager.SetLockoutEnabledAsync(user, false);
        if (!result.Succeeded && !resultDate.Succeeded)
            throw new ExceptionCommonReponse(MessageReturn.Mangement_FailUserUnLock, 400);
        _logger.Info<User>(MessageReturn.Mangement_SuccessUserUnLock, id, AuditType.edit);
        return _iRepositoryMessage.SuccessMessage();

    }
    public async Task<LoginMessage> LoginAs(string id)
    {
        var appUser = await _userManager.FindByIdAsync(id);
        if (appUser == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var users = _mapper.Map<UserListDto>(appUser);
        var token = await RoleJWTToken(appUser);
        var tokenPermision = await PermisionJWTToken(appUser);
        _logger.LogInSuccess(appUser.UserName);
        return SuccessMessage(token, tokenPermision, users);


    }
    public async Task<LoginMessage> GetToken()
    {
        var userId = _accssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var appUser = await _userManager.FindByIdAsync(userId);
        if (appUser == null)
            throw new ExceptionCommonReponse(MessageReturn.Common_NotFound, 400);
        var users = _mapper.Map<UserListDto>(appUser);
        var token = await RoleJWTToken(appUser);
        var tokenPermision = await PermisionJWTToken(appUser);
        _logger.LogInSuccess(appUser.UserName);
        return SuccessMessage(token, tokenPermision, users);
    }
    private async Task<string> RoleJWTToken(User user)
    {
        List<Claim> rolesCliams = new List<Claim>{  // claim is represent info of entity (user),and key value
                   new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                   new Claim(ClaimTypes.Name,user.UserName),
                   new Claim("Type",user.UserType.ToString()),
                   new Claim(ClaimTypes.Surname,user.Name ),

                };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            rolesCliams.Add(new Claim(ClaimTypes.Role, role));
        }
        return GenerateJWTToken(user, rolesCliams, _timeToken.MAXMINUTE);

    }
    
    private async Task<string> PermisionJWTToken(User user)
    {
        List<Claim> rolesCliams = new List<Claim>{  // claim is represent info of entity (user),and key value
                   new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                   };
        var roles = (await _UserRole.GetAllAsync(a => a.UserId == user.Id, r => r.Role)).Select(a => a.Role);
        foreach (var role in roles)
        {
            var permisions = await _roleManager.GetClaimsAsync(role);
            if (permisions.Any())
                rolesCliams.AddRange(permisions);
        }
        var UserPermisions = await _userManager.GetClaimsAsync(user);
        if (UserPermisions.Any())
            rolesCliams.AddRange(UserPermisions);

        if (rolesCliams.Any())
            return GenerateJWTToken(user, rolesCliams.DistinctBy(a => a.Value).ToList(), _timeToken.MAXMINUTEPermission);
        else return string.Empty;

    }
    private string GenerateJWTToken(User user, List<Claim> calims, int time)
    {

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)); // set of characters into a sequence of bytes
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);//Represents the cryptographic key and security algorithms that are used to generate a digital signature.
        var tokenDescripror = new SecurityTokenDescriptor //Contains some information which used to create a security token.
        {
            Subject = new ClaimsIdentity(calims),
            Expires = DateTime.Now.AddMinutes(time),
            SigningCredentials = creds
        };
        var tokenHandler = new JwtSecurityTokenHandler();// designed for creating and validating Json Web Tokens.
        var token = tokenHandler.CreateToken(tokenDescripror);
        return tokenHandler.WriteToken(token);
    }


    private LoginMessage ErrorMessage(string msg, object returnEntity = null, Int16 StatusCode = 400)
    {
        return new LoginMessage
        {
            Status = false,
            Message = string.IsNullOrEmpty(msg) ? "" : _localizer[msg].Value,
            StatusCode = StatusCode
        };
    }
    private LoginMessage SuccessMessage(string token, string tokenPermision, UserListDto User, object returnEntity = null, string msg = "", Int16 StatusCode = 400)
    {
        return new LoginMessage
        {
            ReturnEntity = new { Token = token, tokenPermision = tokenPermision, User = User },
            Status = true,
            Message = string.IsNullOrEmpty(msg) ? "" : _localizer[msg].Value,
            StatusCode = StatusCode
        };
    }
    protected void LogRowRegister(ref User entity)
    {
        // var userName =int.Parse( _accssor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        entity.IPDeviceCreaded = entity.IPDeviceLastEdit = _accssor.HttpContext.Connection.RemoteIpAddress.ToString();
        entity.CreationTime = entity.LastModificationTime = _iClockService.Now;
        // entity.CreatorId = entity.LastModifierId = userName;
        entity.ClientId = entity.ClientId ?? Guid.NewGuid();
    }
}
