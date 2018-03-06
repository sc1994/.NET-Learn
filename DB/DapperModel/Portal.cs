using System;
using DapperModel;
using System.ComponentModel;

namespace DapperDemo
{
    public class Table1 : BaseModel
    {
        public override string PrimaryKey { get; } = "TID";
        public override string IdentityKey { get; } = "AutoID";
        public override string DbName { get; } = "MDCalendar";
        public override string TableName { get; } = "Table_1";
        public override string ConnectionString { get; } = @"Server=172.17.30.145\mingdao3;User ID=sa;Password=mingdao3;Pooling=true;Max Pool Size=32767;Min Pool Size=0;";

        /// <summary>
        /// 自增键
        /// </summary>
        [Description("主键Id")]
        // ReSharper disable once InconsistentNaming
        public int AutoID { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键")]
        // ReSharper disable once InconsistentNaming
        public Guid TID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Description("姓名")]
        // ReSharper disable once InconsistentNaming
        public string beck { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }
    }
}
