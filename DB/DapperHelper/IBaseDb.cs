using DapperModel;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DapperHelper
{
    public interface IBaseDb<TModel> where TModel : BaseModel
    {
        /// <summary>
        /// 插入一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns>结果,自增值</returns>
        (bool result, int identityKey) Insert(TModel model, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 插入一条数据(异步执行)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns>结果,自增值</returns>
        Task<(bool result, int identityKey)> InsertAsync(TModel model, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="models"></param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        int InsertRange(IEnumerable<TModel> models, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 批量插入数据(异步执行)
        /// </summary>
        /// <param name="models"></param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        Task<int> InsertRangeAsync(IEnumerable<TModel> models, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 依赖主键删除
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        bool Delete<TKey>(TKey primaryKey, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 依赖主键删除(异步执行)
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        Task<bool> DeleteAsync<TKey>(TKey primaryKey, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="wheres"></param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        int DeleteRange(Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 批量删除(异步执行)
        /// </summary>
        /// <param name="wheres"></param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        Task<int> DeleteRangeAsync(Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allowInsert">true: 数据不存在时,运行插入数据 false: key没有匹配到数据抛出异常</param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        bool Update(TModel model, bool allowInsert = true, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 更新一条数据(异步执行)
        /// </summary>
        /// <param name="model"></param>
        /// <param name="allowInsert">true: 数据不存在时,运行插入数据 false: key没有匹配到数据抛出异常</param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        Task<bool> UpdateAsync(TModel model, bool allowInsert = true, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="updates">跟新字段</param>
        /// <param name="wheres">条件</param>
        /// <param name="top">0: 不限制更新条数</param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        bool UpdateRange(Update<TModel> updates, Where<TModel> wheres, int top = 0, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 批量更新(异步执行)
        /// </summary>
        /// <param name="updates">跟新字段</param>
        /// <param name="wheres">条件</param>
        /// <param name="top">0: 不限制更新条数</param>
        /// <param name="allowLog">是否记录日志</param>
        /// <param name="transaction">事务</param>
        /// <returns></returns>
        Task<bool> UpdateRangeAsync(Update<TModel> updates, Where<TModel> wheres, int top = 0, bool allowLog = false, IDbTransaction transaction = null);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="primaryKey"></param>
        /// <returns></returns>
        bool Exist<TKey>(TKey primaryKey);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="identityKey"></param>
        /// <returns></returns>
        bool ExistByIdentityKey(int identityKey);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="wheres"></param>
        /// <returns></returns>
        bool Exist(Where<TModel> wheres);

        /// <summary>
        /// 获取数据条数
        /// </summary>
        /// <param name="wheres"></param>
        /// <returns></returns>
        long Count(Where<TModel> wheres);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="primaryKey">主键</param>
        /// <returns></returns>
        TModel Get<TKey>(TKey primaryKey);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="identityKey"></param>
        /// <returns></returns>
        TModel GetByIdentityKey(int identityKey);

        /// <summary>
        /// 获取单条数据
        /// </summary>
        /// <param name="wheres">条件</param>
        /// <returns></returns>
        TModel Get(Where<TModel> wheres);

        /// <summary>
        /// 获取批量数据
        /// </summary>
        /// <param name="wheres">条件</param>
        /// <param name="shows">需要获取的字段</param>
        /// <param name="orders">排序</param>
        /// <returns></returns>
        IEnumerable<TModel> GetRange(Where<TModel> wheres, Show<TModel> shows = null, Order<TModel> orders = null);

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="wheres">条件</param>
        /// <param name="orders">页排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="shows">需要获取的字段</param>
        /// <param name="pageOrders">当前页排序</param>
        /// <returns></returns>
        IEnumerable<TModel> GetPage(Where<TModel> wheres, Order<TModel> orders, int pageIndex, int pageSize, Show<TModel> shows = null, Order<TModel> pageOrders = null);
    }
}
