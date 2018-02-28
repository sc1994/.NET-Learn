using DapperModel;
using System.Collections.Generic;

namespace DapperHelper
{
    public static class PersonData
    {
        public static Value Add(this Value that, string msg)
        {
            that.Ha.Add(msg);
            return that;
        }
    }

    public class Value
    {
        internal List<string> Ha { get; set; } = new List<string>();
    }

    public abstract class BaseDb<TModel, TEnum, TKey> : IBaseDb<TModel, TEnum, TKey>
        where TModel : class
    {
        protected string PrimaryKey { get; }
        protected string IdentityKey { get; }
        protected string DbName { get; }
        protected string TableName { get; }

        protected BaseDb(string primaryKey, string identityKey, string dbName, string tableName)
        {
            PrimaryKey = primaryKey;
            IdentityKey = identityKey;
            DbName = dbName;
            TableName = tableName;
        }

        public bool IsExist(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool IsExist(Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public long Count(Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TModel model, bool log = false)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Dictionary<TEnum, object> update, Where<TModel> wheres, int top = 0)
        {
            throw new System.NotImplementedException();
        }

        public TKey Insert(TModel model)
        {
            throw new System.NotImplementedException();
        }

        public void InsertMany(IList<TModel> models)
        {
            throw new System.NotImplementedException();
        }

        public TModel Get(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public TModel Get(Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public TModel Get(Show<TModel> shows, Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetList(Where<TModel> wheres, Sort<TModel> orders = null)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetList(Show<TModel> shows, Where<TModel> wheres, Sort<TModel> orders = null)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetPage(Show<TModel> shows, Where<TModel> wheres, Sort<TModel> orders, int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetPage()
        {
            throw new System.NotImplementedException();
        }
    }

    public interface IBaseDb<TModel, TEnum, TKey> where TModel : class
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
