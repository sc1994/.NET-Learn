using System;
using System.ComponentModel;

namespace DapperModel
{
    public class Person : BaseModel
    {
        public override string PrimaryKey { get; } = "IdNumber";
        public override string IdentityKey { get; } = "AutoId";
        public override string DbName { get; } = "Learn";
        public override string TableName { get; } = "Person";
        public override string ConnectionString { get; } = @"Server=118.24.27.231\SQLEXPRESS;User ID=sa;Password=sun940622;Pooling=true;Max Pool Size=32767;Min Pool Size=0;";

        /// <summary>
        /// 自增键
        /// </summary>
        [Description("主键Id")]
        // ReSharper disable once InconsistentNaming
        public int AutoId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        [Description("主键")]
        // ReSharper disable once InconsistentNaming
        public Guid IdNumber { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 姓名
        /// </summary>
        [Description("姓名")]
        // ReSharper disable once InconsistentNaming
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("生日")]
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Description("性别")]
        public int Sex { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Description("联系电话")]
        public string Phone { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Description("地址")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// 是否删除
        /// </summary>
        [Description("是否删除")]
        public bool IsDelete { get; set; } = false;
    }

    public class SysLog
    {
        public int AutoId { get; set; }
        public string Describe { get; set; } = string.Empty;
        public DateTime ExecuteTime { get; set; } = DateTime.Now;
        public string StackTrace { get; set; } = string.Empty;
        public string Sql { get; set; } = string.Empty;
        public string Params { get; set; } = string.Empty;
        public string OldValue { get; set; } = string.Empty;
        public string NewValue { get; set; } = string.Empty;
    }
}
