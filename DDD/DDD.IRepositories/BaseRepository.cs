using System.Collections.Generic;

namespace DDD.IRepositories
{
    public class BaseRepository<TModel> : IBaseRepository<TModel> where TModel : new()
    {
        public virtual int Count()
        {
            return 111;
        }

        public virtual IEnumerable<TModel> GetList()
        {
            return new[] {new TModel(), new TModel(), new TModel()};
        }
    }
}
