using DDD.Entity;

namespace DDD.IRepositories
{
    public interface ITable2Repository : IBaseRepository<Table2Entity>
    {
        bool Update();
    } 
}
