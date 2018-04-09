using DDD.Entity;

namespace DDD.IRepositories
{
    public interface ITableRepository : IBaseRepository<TableEntity>
    {
        bool Delete();
    }
}
