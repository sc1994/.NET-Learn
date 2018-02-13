using DapperExtensions;
using System.Collections.Generic;
using Dapper;

namespace DapperDemo
{
    interface IBaseDb<TModel, TEnum, TKey> where TModel : new()
    {
        bool IsExist(TKey key);

        bool IsExist(params IBasePredicate[] wheres);

        long Count(params IBasePredicate[] wheres);

        bool Update(TModel model, bool log = false);

        /// <summary>
        /// 依据自定义条件和自定义字段更新
        /// </summary>
        /// <param name="update"></param>
        /// <param name="wheres"></param>
        /// <param name="top">默认不限制更新条数</param>
        /// <returns></returns>
        bool Update(Dictionary<TEnum, object> update, IBasePredicate[] wheres, int top = 0);

        TKey Insert(TModel model);

        void InsertMany(IList<TModel> models);

        TModel Get(TKey key);

        TModel Get(params IBasePredicate[] wheres);

        TModel Get(IBasePredicate[] wheres, params TEnum[] shows);

        IList<TModel> GetList(params IBasePredicate[] wheres);

        IList<TModel> GetList(IBasePredicate[] wheres, params TEnum[] shows);

        IList<TModel> GetPage(TEnum[] shows, IBasePredicate[] wheres, TEnum orders, int pageIndex, int pageSize);

        IList<TModel> GetPage();
    }


    public abstract class BaseDb<TModel, TEnum, TKey> : IBaseDb<TModel, TEnum, TKey>
        where TModel : BaseModel, new()
    {
        public long Count(params IBasePredicate[] wheres)
        {
            using (var cn = DataSource.GetExtensionsConnection())
            {
                return cn.Count<TModel>(wheres);
            }
        }

        public TModel Get(TKey key)
        {
            var info = new TModel();
            var sql = $@"SELECT * FROM {info.DbName}.{info.TableName} WHERE {info.PrimaryKey} = @PrimaryKey";
            using (var cn = DataSource.GetDapperConnection())
            {
                return cn.QueryFirst<TModel>(sql,
                                             new
                                             {
                                                 PrimaryKey = key
                                             });
            }
        }

        public TModel Get(params IBasePredicate[] wheres)
        {
            using (var cn = DataSource.GetExtensionsConnection())
            {
                return cn.Get<TModel>(wheres);
            }
        }

        public TModel Get(IBasePredicate[] wheres, params TEnum[] shows)
        {
            using (var cn = DataSource.GetExtensionsConnection())
            {
                return cn.Get<TModel>();
            }
        }

        public IList<TModel> GetList(params IBasePredicate[] wheres)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetList(IBasePredicate[] wheres, params TEnum[] shows)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetPage(TEnum[] shows, IBasePredicate[] wheres, TEnum orders, int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetPage()
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

        public bool IsExist(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool IsExist(params IBasePredicate[] wheres)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(TModel model, bool log = false)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Dictionary<TEnum, object> update, IBasePredicate[] wheres, int top = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}
