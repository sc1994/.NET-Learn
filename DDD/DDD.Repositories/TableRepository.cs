using System;
using DDD.Entity;
using DDD.IRepositories;

namespace DDD.Repositories
{
    public class TableRepository : BaseRepository<TableEntity>, ITableRepository
    {
        public static TableRepository Instance => new Lazy<TableRepository>(() => new TableRepository()).Value;

        public bool Delete()
        {
            return true;
        }
    }
}
