using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services.Common;
using Core.Entities.Management;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Core.Interfaces.Common;
using Core.Enums;
namespace Infrastructure.Repository
{
    public class RepositoryApp<T> : IRepositoryApp<T> where T : class
    {
        protected readonly AppDbContext _db;
        private readonly ILogCustom _iLogCustom;
        protected DbSet<T> Entity = null;

        public RepositoryApp(
            AppDbContext db,
            ILogCustom ILogCustom
            )
        {
            _db = db;
            _iLogCustom = ILogCustom;
            Entity = _db.Set<T>();
        }
        #region main operation
        public void Add(T entity)
        {
            Entity.Add(entity);

        }
        public void AddRange(List<T> entities)
        {
            Entity.AddRange(entities);
        }
        public void UpdateRange(IEnumerable<T> entities)
        {
            Entity.UpdateRange(entities);
        }
        public void Update(T entity)
        {
            var x = Entity.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
        }
        public void Delete(T entity)
        {
            Entity.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            Entity.RemoveRange(entities);
        }

        #endregion

        #region GetALL Async =>(IQueryable)
        public IQueryable<T> GetAll()
        {
            return Entity;
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(i => true);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return result;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return Entity.Where(whereCondition);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);
            return result;
        }

        #endregion

        #region GetALL Async =>(IQueryable)
        public async Task<double> Count()
        {
            return await Entity.CountAsync();
        }

        public async Task<double> Count(params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(i => true);

            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.CountAsync();
        }

        public async Task<double> Count(Expression<Func<T, bool>> whereCondition)
        {
            return await Entity.Where(whereCondition).CountAsync();
        }

        public async Task<double> Count(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);
            return await result.CountAsync(); ;
        }

        #endregion


        #region GetALL Async =>(IEnumerable)

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entity.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(i => true);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);
            return await result.ToListAsync(); ;
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Entity.Where(whereCondition).ToListAsync();

        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);
            return await result.ToListAsync();
        }

        #endregion

        #region GetById Async  

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await Entity.FindAsync(id);

        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await Entity.FindAsync(id);

        }
        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);
            return await result.FirstOrDefaultAsync();

        }
         public  T GetById(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);
            return   result.FirstOrDefault();

        }



        #endregion

        #region  SingleOrDefaultAsync
        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Entity.Where(whereCondition).FirstOrDefaultAsync();
        }
        public  T SingleOrDefault(Expression<Func<T, bool>> whereCondition)
        {
            return   Entity.Where(whereCondition).FirstOrDefault();
        }
        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.FirstOrDefaultAsync();
        }
        public async Task<T> SingleOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> whereCondition)
        {
            return await Entity.Where(whereCondition).AsNoTracking().FirstOrDefaultAsync();
        }
         public    T SingleOrDefaultAsNoTracking(Expression<Func<T, bool>> whereCondition)
        {
            return   Entity.Where(whereCondition).AsNoTracking().FirstOrDefault();
        }
        public async Task<T> SingleOrDefaultAsNoTrackingAsync(Expression<Func<T, bool>> whereCondition, params Expression<Func<T, object>>[] includes)
        {
            var result = Entity.Where(whereCondition);
            foreach (var includeExpression in includes)
                result = result.Include(includeExpression);

            return await result.AsNoTracking().FirstOrDefaultAsync();
        }
        
        #endregion

        #region OrderBy
        public IEnumerable<T> OrderBy(Expression<Func<T, bool>> whereCondition)
        {
            return Entity.OrderBy(whereCondition);
        }
        public IEnumerable<T> OrderByDescending(Expression<Func<T, bool>> whereCondition)
        {
            return Entity.OrderByDescending(whereCondition);
        }
        #endregion


        #region SaveAllAsync
        public async Task<bool> SaveAllAsync(bool statusAudit=true)
        {
            try{
            if(statusAudit)
              OnBeforeSaveChanges();
            }catch(Exception){};
            return await _db.SaveChangesAsync() > 0;


        }
         public  bool SaveAll(bool statusAudit=true)
        {
             
              try{
            if(statusAudit)
              OnBeforeSaveChanges();
            }catch(Exception){};
            return   _db.SaveChanges() > 0;
        }


        #endregion
        #region checkState
        public bool checkState(T entity, string state)
        {
            var x = _db.Entry(entity).State;
            return (_db.Entry(entity).State.ToString().ToLower() == state.ToLower().Trim()) ? true : false;
        }

        public DbSet<T> GetContext()
        {
            return Entity;

        }

        public async Task<T> FirstAsync()
        {
            return await Entity.FirstOrDefaultAsync();
        }
        public async Task<T> FirstAsNoTrackingAsync()
        {
            return await Entity.AsNoTracking().FirstOrDefaultAsync();
        }
        public IQueryable<T> GetFromProcedures(string sql, params object[] parameters)
        {
            return Entity.FromSqlRaw(sql).IgnoreQueryFilters();
        }


        #endregion


        private void OnBeforeSaveChanges()
        {
            _db.ChangeTracker.DetectChanges();
            foreach (var entry in _db.ChangeTracker.Entries())
            {
                var auditEntry = new Audit();
                if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                    continue;
                auditEntry.TableName = entry.Entity.GetType().Name;
                auditEntry.PrimaryKeyObj= entry.Properties.FirstOrDefault(a=>a.Metadata.IsPrimaryKey()).CurrentValue;
                 auditEntry.RowClientId = entry.Properties.FirstOrDefault(a => a.Metadata.Name.ToLower() == "clientId".ToLower()).CurrentValue.ToString();
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.State = AuditType.register.ToString();
                        var valuesAdded = entry.Entity;
                        auditEntry.NewValues  = JsonConvert.SerializeObject(entry.Entity);
                        break;
                    case EntityState.Deleted:
                        auditEntry.State = AuditType.delete.ToString();
                          auditEntry.OldValues  = JsonConvert.SerializeObject(entry.Entity);
                      
                        break;
                    case EntityState.Modified:
                          auditEntry.State = AuditType.edit.ToString();
                          List<PropertyEntry>ss=new List<PropertyEntry>();
                         var properties= entry.Properties;
                          if (properties.Any(a=>a.IsModified))
                        {
                            var oldValuesModified =properties.Select(a=> new {a.Metadata.Name,value=a.OriginalValue}).ToDictionary(a=>a.Name,v=>v.value);
                             auditEntry.OldValues = oldValuesModified.Count() == 0 ? null : JsonConvert.SerializeObject(oldValuesModified);
                            var newValuesModified = properties.Select(a=> new {a.Metadata.Name,value=a.CurrentValue}).ToDictionary(a=>a.Name,v=>v.value);
                            auditEntry.NewValues = newValuesModified.Count() == 0 ? null : JsonConvert.SerializeObject(newValuesModified);
                        }
                        break;
                }
         _iLogCustom.Info(auditEntry);
         }
         }

       
    }
}
