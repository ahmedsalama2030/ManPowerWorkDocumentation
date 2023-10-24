using System.Reflection;
using Core.Entities.Management;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Core.Entities.Chat;
using Core.Entities.Request;
namespace Infrastructure.Data;
public partial class AppDbContext : IdentityDbContext<User, Role, int, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
{
    public AppDbContext(DbContextOptions<AppDbContext> options
    ) : base(options)
    { }
    #region 
    // public DbSet<RequestDocumentation> RequestDocumentation { get; set; }
    #endregion
    #region  chats
    public DbSet<Chat> Chats { get; set; }
    #endregion s
    #region  Management
    public DbSet<Language> Languages { get; set; }
    public DbSet<LanguageKey> LanguageKeys { get; set; }
    public DbSet<LanguageText> LanguageTexts { get; set; }
    public DbSet<ScreenApp> ScreensApp { get; set; }
    public DbSet<ModuleApp> ModulesApp { get; set; }
    public DbSet<ClaimApp> ClaimsApp { get; set; }
    public DbSet<LanguageKeysView> ClaimAppView { get; set; }
    public DbSet<ClaimAppView> ClaimAppSP { get; set; }
    #endregion
     
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<ScreenApp>(e => e.HasQueryFilter(a => !a.IsDeleted));
        modelBuilder.Entity<ModuleApp>(e => e.HasQueryFilter(a => !a.IsDeleted));
        modelBuilder.Entity<ClaimApp>(e => e.HasQueryFilter(a => !a.IsDeleted));
        modelBuilder.Entity<Chat>(e => e.HasQueryFilter(a => !a.IsDeleted));
        modelBuilder.Entity<LanguageKey>(e => e.HasQueryFilter(a => !a.IsDeleted));
        modelBuilder.Entity<LanguageText>(e => e.HasQueryFilter(a => !a.IsDeleted));
        modelBuilder.Entity<LanguageKeysView>(eb =>
        {
            eb.HasNoKey();
            eb.ToView("LanguageKeysView", schema: "Management");
        });
        modelBuilder.Entity<ClaimAppView>(eb =>
        {
            eb.ToView("ClaimAppView", schema: "Management");
        });

 
            
    }
}

