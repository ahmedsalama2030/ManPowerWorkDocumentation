using System.Threading.Tasks;
using Core.Interfaces;
using Core.Interfaces.Common;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class UnitOfWork<T>: IUnitOfWork<T> where T : class
    {
        private IRepositoryApp<T> _entity;

        private readonly AppDbContext _db;
        private readonly ILogCustom _iLogCustom;

        public UnitOfWork(AppDbContext db,ILogCustom ILogCustom)
        {
            _db = db;
            _iLogCustom = ILogCustom;
        }
        public IRepositoryApp<T> Table
        {
            get
            {
                return _entity ?? (_entity = new RepositoryApp<T>(_db,_iLogCustom));
            }
        }
        public async Task<bool> SaveAllAsync()
        {
          return await _db.SaveChangesAsync()>0;
        }

        public bool SaveAll()
        {
         return   _db.SaveChanges()>0;

        }

    }
}