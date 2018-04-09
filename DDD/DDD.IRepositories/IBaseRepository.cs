using System.Collections.Generic;

namespace DDD.IRepositories
{
    public interface IBaseRepository<out TModel> where TModel : new()
    {
        int Count();

        IEnumerable<TModel> GetList();
    }
}
