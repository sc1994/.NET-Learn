using Utilities;
using System.Data;
using System.Data.SqlClient;

namespace DapperDemo
{
    /// <summary>
    /// 数据源
    /// </summary>
    public class DataSource
    {
        private static string _sqlConnection;
        /// <summary>
        /// 连接池
        /// </summary>
        /// <returns></returns>
        public static IDbConnection GetDapperConnection()
        {
            InitConfig();
            return new SqlConnection(_sqlConnection);
        }

        public static SqlConnection GetExtensionsConnection()
        {
            InitConfig();
            return new SqlConnection(_sqlConnection);
        }

        private static void InitConfig()
        {
            if (string.IsNullOrEmpty(_sqlConnection))
            {
                if (string.IsNullOrEmpty(ConfigHelper.Get("SqlConnection")))
                {
                    throw new NoNullAllowedException("请配置数据库的连接");
                }
                _sqlConnection = ConfigHelper.Get("SqlConnection");
            }
        }
    }
}