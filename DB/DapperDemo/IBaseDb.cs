using Utilities;
using System.Collections.Generic;

namespace DapperDemo
{
    public interface IBaseDb<TModel, TEnum, TKey> where TModel : ModelBase
    {
        bool IsExist(TKey key);

        bool IsExist(Where<TModel> wheres);

        long Count(Where<TModel> wheres);

        bool Update(TModel model, bool log = false);

        /// <summary>
        /// 依据自定义条件和自定义字段更新
        /// </summary>
        /// <param name="update"></param>
        /// <param name="wheres"></param>
        /// <param name="top">默认不限制更新条数</param>
        /// <returns></returns>
        bool Update(Dictionary<TEnum, object> update, Where<TModel> wheres, int top = 0);

        TKey Insert(TModel model);

        void InsertMany(IList<TModel> models);

        TModel Get(TKey key);

        TModel Get(Where<TModel> wheres);

        TModel Get(Show<TModel> shows, Where<TModel> wheres);

        IList<TModel> GetList(Where<TModel> wheres, Sort<TModel> orders = null);

        IList<TModel> GetList(Show<TModel> shows, Where<TModel> wheres, Sort<TModel> orders = null);

        IList<TModel> GetPage(Show<TModel> shows, Where<TModel> wheres, Sort<TModel> orders, int pageIndex, int pageSize);

        IList<TModel> GetPage();
    }
}
