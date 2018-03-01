using System;
using Dapper;
using System.Data;
using System.Text;
using DapperModel;
using System.Linq;
using static System.String;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DapperHelper
{
    public class BaseDb<TModel> : IBaseDb<TModel> where TModel : BaseModel
    {
        private readonly TModel _model = Activator.CreateInstance<TModel>();

        private static readonly Lazy<BaseDb<TModel>> Lazy = new Lazy<BaseDb<TModel>>(() => new BaseDb<TModel>());

        public static BaseDb<TModel> Instance => Lazy.Value;

        public (bool result, int identityKey) Insert(TModel model, bool allowLog = false, IDbTransaction transaction = null)
        {
            var sql = GetInsertSql();
            if (!IsNullOrEmpty(model.IdentityKey))
            {
                sql += "\r\nSELECT @@IDENTITY;";
            }

            var result = Query<int>(sql, model).FirstOrDefault();
            return (result > 0, result);
        }

        public async Task<(bool result, int identityKey)> InsertAsync(TModel model, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => Insert(model, allowLog, transaction));
        }

        public int InsertRange(IList<TModel> models, bool allowLog = false, IDbTransaction transaction = null)
        {
            return Execute(GetInsertSql(), models);
        }

        public async Task<int> InsertRangeAsync(IList<TModel> models, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => InsertRange(models, allowLog, transaction));
        }

        private string GetInsertSql()
        {
            var sql = new StringBuilder();
            var properties = _model.GetType().GetProperties();
            var fields = Empty;
            var values = Empty;
            var otherField = new[] { "PrimaryKey", "IdentityKey", "DbName", "TableName", "ConnectionString" };
            foreach (var t in properties.Where(x => !otherField.Contains(x.Name)))
            {
                if (!IsNullOrEmpty(_model.IdentityKey) && _model.IdentityKey == t.Name)
                    continue;

                fields += $"{t.Name},";
                values += $"@{t.Name},";
            }

            sql.AppendLine("INSERT INTO");
            sql.AppendLine($"[{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("(");
            sql.AppendLine(fields.TrimEnd(','));
            sql.AppendLine(")");
            sql.AppendLine("VALUES");
            sql.AppendLine("(");
            sql.AppendLine(values.TrimEnd(','));
            sql.AppendLine(");");

            return sql.ToString();
        }

        public bool Delete<TKey>(TKey primaryKey, bool allowLog = false, IDbTransaction transaction = null)
        {
            var tuple = GetDeleteSql();
            return Execute()
        }

        public Task<bool> DeleteAsync<TKey>(TKey primaryKey, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public int DeleteRange(Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteRangeAsync(Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        private (string sql, object param) GetDeleteSql(Where<TModel> wheres)
        {
            var sql = new StringBuilder();
            var tuple = GetWhereSql(wheres);
            sql.AppendLine($"DELETE FROM [{_model.DbName}].[{_model.TableName}]");
            sql.AppendLine("WHERE 1=1");
            sql.AppendLine(tuple.sql);
            return (sql.ToString(), tuple.param);
        }

        public bool Update(TModel model, bool allowInsert = true, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Update<TModel> updates, Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(TModel model, bool allowInsert = true, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(Update<TModel> updates, Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateRange(Update<TModel> updates, Where<TModel> wheres, int top = 0, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateRangeAsync(Update<TModel> updates, Where<TModel> wheres, int top = 0, bool allowLog = false, IDbTransaction transaction = null)
        {
            throw new System.NotImplementedException();
        }

        public bool Exist<TKey>(TKey primaryKey)
        {
            throw new System.NotImplementedException();
        }

        public bool ExistByIdentityKey(int identityKey)
        {
            throw new System.NotImplementedException();
        }

        public bool Exist(Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public bool Count(Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public TModel Get<TKey>(TKey primaryKey)
        {
            throw new System.NotImplementedException();
        }

        public bool GetByIdentityKey(int identityKey)
        {
            throw new System.NotImplementedException();
        }

        public TModel Get(Where<TModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetRange(Where<TModel> wheres, Show<TModel> shows = null, Order<TModel> orders = null)
        {
            throw new System.NotImplementedException();
        }

        public IList<TModel> GetPage(Where<TModel> wheres, Order<TModel> orders, int pageIndex, int pageSize, Show<TModel> shows = null, Order<TModel> pageOrders = null)
        {
            throw new System.NotImplementedException();
        }


        private (string sql, object param) GetWhereSql(Where<TModel> wheres)
        {
            var whereDictionary = InitWhere<TModel>.GetWhere(wheres);
            var sql = new StringBuilder();
            var param = new DynamicParameters();
            for (var i = 0; i <= whereDictionary.Count; i++)
            {
                var where = whereDictionary[i];
                var paramName = $"{where.FieldDictionary.Parent}_{where.FieldDictionary.Name}_{i}";
                sql.AppendLine($"{where.Coexist} {where.FieldDictionary.Parent}.{where.FieldDictionary.Name} {where.Relation} {paramName}");
                param.Add(paramName, where.Value);
            }
            return (sql.ToString(), param);
        }

        /// <summary>
        /// 执行带参查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private IEnumerable<T> Query<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using (var con = new SqlConnection(_model.ConnectionString))
            {
                try
                {
                    return con.Query<T>(sql, param, transaction);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " \r\n ------------------------ SQL: \r\n" + sql);
                }
            }
        }

        /// <summary>
        /// 执行受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private int Execute(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using (var con = new SqlConnection(_model.ConnectionString))
            {
                try
                {
                    return con.Execute(sql, param, transaction);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + " \r\n ------------------------ SQL: \r\n" + sql);
                }
            }
        }
    }
}
