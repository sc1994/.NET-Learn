using System;
using Dapper;
using Utilities;
using System.Data;
using System.Text;
using DapperModel;
using System.Linq;
using static System.String;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;

namespace DapperHelper
{
    public class BaseDb<TModel> : IBaseDb<TModel> where TModel : BaseModel
    {
        private readonly TModel _model = Activator.CreateInstance<TModel>();

        private static readonly Lazy<IBaseDb<TModel>> Lazy = new Lazy<IBaseDb<TModel>>(() => new BaseDb<TModel>());

        public static IBaseDb<TModel> Instance => Lazy.Value;

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

        public int InsertRange(IEnumerable<TModel> models, bool allowLog = false, IDbTransaction transaction = null)
        {
            return Execute(GetInsertSql(), models);
        }

        public async Task<int> InsertRangeAsync(IEnumerable<TModel> models, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => InsertRange(models, allowLog, transaction));
        }

        private Dictionary<string, object> GetValues(TModel model, params string[] fields)
        {
            var result = new Dictionary<string, object>();
            var properties = model.GetType().GetProperties();

            foreach (var t in properties)
            {
                if (!fields.Contains(t.Name))
                    throw new Exception(nameof(fields));
                result.Add(t.Name, t.GetValue(model, null));
            }

            return result;
        }

        public bool Delete<TKey>(TKey primaryKey, bool allowLog = false, IDbTransaction transaction = null)
        {
            var sql = $"DELETE FROM [{_model.DbName}].dbo.[{_model.TableName}] WHERE {_model.PrimaryKey} = @primaryKey";
            return Execute(sql, new { primaryKey }) > 0;
        }

        public async Task<bool> DeleteAsync<TKey>(TKey primaryKey, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => Delete(primaryKey, allowLog, transaction));
        }

        public int DeleteRange(Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null)
        {
            var tuple = GetDeleteSql(wheres);
            return Execute(tuple.sql, tuple.param);
        }

        public async Task<int> DeleteRangeAsync(Where<TModel> wheres, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => DeleteRange(wheres, allowLog, transaction));
        }

        private (string sql, object param) GetDeleteSql(Where<TModel> wheres)
        {
            var sql = new StringBuilder();
            var tuple = GetWhereSql(wheres);
            sql.AppendLine($"DELETE FROM [{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("WHERE 1=1");
            sql.AppendLine(tuple.sql);
            return (sql.ToString(), tuple.param);
        }

        public bool Update(TModel model, bool allowInsert = true, bool allowLog = false, IDbTransaction transaction = null)
        {
            var sql = new StringBuilder();
            var fields = GetFields();

            sql.AppendLine($"UPDATE TOP(1) [{model.DbName}].dbo.[{model.TableName}] SET");
            sql.AppendLine(fields.Aggregate(Empty, (current, item) => $"{current}, {item}= @{item}").TrimStart(','));
            sql.AppendLine($"WHERE {model.PrimaryKey} = @{model.PrimaryKey};");

            var param = new DynamicParameters();
            var values = GetValues(model, model.PrimaryKey);
            param.Add(model.PrimaryKey, values[model.PrimaryKey]);

            return Execute(sql.ToString(), param) > 0;
        }

        public async Task<bool> UpdateAsync(TModel model, bool allowInsert = true, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => Update(model, allowInsert, allowLog, transaction));
        }

        public bool UpdateRange(Update<TModel> updates, Where<TModel> wheres, int top = 0, bool allowLog = false, IDbTransaction transaction = null)
        {
            var sql = new StringBuilder();
            var tupleWhere = GetWhereSql(wheres);
            var tupleUpdate = GetUpdateSql(updates);

            sql.AppendLine($"UPDATE{(top == 0 ? "" : $" TOP({top})")} [{_model.DbName}].dbo.[{_model.TableName}] SET");
            sql.AppendLine(tupleUpdate.sql);
            sql.AppendLine("WHERE 1=1 ");
            sql.AppendLine(tupleWhere.sql);

            var param = tupleWhere.param;
            param.AddDynamicParams(tupleUpdate.param);

            return Execute(sql.ToString(), param) > 0;
        }

        public async Task<bool> UpdateRangeAsync(Update<TModel> updates, Where<TModel> wheres, int top = 0, bool allowLog = false, IDbTransaction transaction = null)
        {
            return await Task.Run(() => UpdateRange(updates, wheres, top, allowLog, transaction));
        }

        public bool Exist<TKey>(TKey primaryKey)
        {
            var sql = $"SELECT COUNT(1) FROM [{_model.DbName}].dbo.[{_model.TableName}] WHERE {_model.PrimaryKey} = @primaryKey";
            return QueryFirst<int>(sql, new { primaryKey }) > 0;
        }

        public bool ExistByIdentityKey(int identityKey)
        {
            var sql = $"SELECT COUNT(1) FROM [{_model.DbName}].dbo.[{_model.TableName}] WHERE {_model.IdentityKey} = @identityKey";
            return QueryFirst<int>(sql, new { identityKey }) > 0;
        }

        public bool Exist(Where<TModel> wheres)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT COUNT(1) FROM");
            sql.AppendLine($"[{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("WHERE 1 = 1");
            var tuple = GetWhereSql(wheres);
            sql.AppendLine(tuple.sql);
            return QueryFirst<long>(sql.ToString(), tuple.param) > 0;
        }

        public long Count(Where<TModel> wheres)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT COUNT(1) FROM");
            sql.AppendLine($"[{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("WHERE 1 = 1");
            var tuple = GetWhereSql(wheres);
            sql.AppendLine(tuple.sql);
            return QueryFirst<long>(sql.ToString(), tuple.param);
        }

        public TModel Get<TKey>(TKey primaryKey)
        {
            var sql = $"SELECT * FROM [{_model.DbName}].dbo.[{_model.TableName}] WHERE {_model.PrimaryKey} = @primaryKey";
            return QueryFirst<TModel>(sql, new { primaryKey });
        }

        public TModel GetByIdentityKey(int identityKey)
        {
            var sql = $"SELECT * FROM [{_model.DbName}].dbo.[{_model.TableName}] WHERE {_model.IdentityKey} = @identityKey";
            return QueryFirst<TModel>(sql, new { identityKey });
        }

        public TModel Get(Where<TModel> wheres)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT * FROM");
            sql.AppendLine($"[{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("WHERE 1 = 1");
            var tuple = GetWhereSql(wheres);
            sql.AppendLine(tuple.sql);
            return QueryFirst<TModel>(sql.ToString(), tuple.param);
        }

        public IEnumerable<TModel> GetRange(Where<TModel> wheres, Show<TModel> shows = null, Order<TModel> orders = null)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SELECT");
            if (shows != null)
            {
                var showFields = InitShow<TModel>.GetShow(shows);
                sql.AppendLine(showFields.Aggregate(Empty, (current, item) => $"{current}, {item.Parent}.{item.Name}"));
            }
            else
            {
                sql.AppendLine("*");
            }
            sql.AppendLine("FROM");
            sql.AppendLine($"[{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("WHERE 1 = 1");
            var tuple = GetWhereSql(wheres);
            sql.AppendLine(tuple.sql);
            if (orders != null)
            {
                var orderFields = InitOrder<TModel>.GetOrder(orders);
                sql.AppendLine(orderFields.Aggregate(Empty, (current, item) => $"{current} {item.FieldDictionary.Name} {item.Sort.ToDescription()}"));
            }
            sql.Append(";");
            return Query<TModel>(sql.ToString(), tuple.param);
        }

        public IEnumerable<TModel> GetPage(Where<TModel> wheres, Order<TModel> orders, int pageIndex, int pageSize, Show<TModel> shows = null, Order<TModel> pageOrders = null)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 获取插入sql
        /// </summary>
        /// <returns></returns>
        private string GetInsertSql()
        {
            var sql = new StringBuilder();
            var fields = GetFields();

            sql.AppendLine("INSERT INTO");
            sql.AppendLine($"[{_model.DbName}].dbo.[{_model.TableName}]");
            sql.AppendLine("(");
            sql.AppendLine(Join(" ,", fields));
            sql.AppendLine(")");
            sql.AppendLine("VALUES");
            sql.AppendLine("(");
            sql.AppendLine($"@{Join(" ,@", fields)}");
            sql.AppendLine(");");

            return sql.ToString();
        }

        /// <summary>
        /// 获取当前 TModel 的字段列表
        /// </summary>
        /// <returns></returns>
        private List<string> GetFields()
        {
            var properties = _model.GetType().GetProperties();

            var otherField = new[] { "PrimaryKey", "DbName", "TableName", "ConnectionString", "IdentityKey", _model.IdentityKey };

            var list = new List<string>();
            foreach (var x in properties)
            {
                if (!otherField.Contains(x.Name))
                {
                    list.Add($"{x.Name}");
                }
            }

            return list;
        }

        /// <summary>
        /// 获取 where 部分的sql,以及参数值
        /// </summary>
        /// <param name="wheres"></param>
        /// <returns></returns>
        private (string sql, DynamicParameters param) GetWhereSql(Where<TModel> wheres)
        {
            var whereDictionary = InitWhere<TModel>.GetWhere(wheres);
            var sql = new StringBuilder();
            var param = new DynamicParameters();

            for (var i = 0; i < whereDictionary.Count; i++)
            {
                var where = whereDictionary[i];
                var paramName = $"@{where.FieldDictionary.Parent}_{where.FieldDictionary.Name}_{i}";
                sql.AppendLine($"   {where.Coexist} {where.FieldDictionary.Name} {Format(where.Relation.ToDescription(), paramName)}");
                param.Add(paramName, where.Value);
            }

            return (sql.ToString(), param);
        }

        /// <summary>
        /// 获取 update 部分的sql,以及参数值
        /// </summary>
        /// <param name="updates"></param>
        /// <returns></returns>
        private (string sql, DynamicParameters param) GetUpdateSql(Update<TModel> updates)
        {
            var updateDictionart = InitUpdate<TModel>.GetUpdate(updates);
            var sql = new StringBuilder();
            var param = new DynamicParameters();

            for (var i = 0; i <= updateDictionart.Count; i++)
            {
                var update = updateDictionart[i];
                var paramName = $"update_{update.FieldDictionary.Parent}_{update.FieldDictionary.Name}_{i}";
                sql.AppendLine($"{update.FieldDictionary.Name} = @{paramName},");
                param.Add(paramName, update.Value);
            }

            return (sql.ToString().TrimEnd(','), param);
        }

        /// <summary>
        /// 执行列表查询
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
        /// 执行单条查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private T QueryFirst<T>(string sql, object param = null, IDbTransaction transaction = null)
        {
            if (IsNullOrEmpty(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }
            using (var con = new SqlConnection(_model.ConnectionString))
            {
                try
                {
                    return con.QueryFirst<T>(sql, param, transaction);
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
