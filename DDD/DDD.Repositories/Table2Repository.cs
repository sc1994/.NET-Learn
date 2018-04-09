using System;
using DDD.Entity;
using DDD.IRepositories;

namespace DDD.Repositories
{
    public class Table2Repository : BaseRepository<Table2Entity>, ITable2Repository
    {
        public static Table2Repository Instance => new Lazy<Table2Repository>(() => new Table2Repository()).Value;

        public bool Update()
        {
            return false;
        }
    }
}
