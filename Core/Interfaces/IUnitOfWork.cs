using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork <T> where T : class
    {
         IRepositoryApp<T> Table{get;}
         Task <bool> SaveAllAsync();
          bool SaveAll();
    }
}