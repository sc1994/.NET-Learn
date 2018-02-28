using System;
using Utilities;
using DapperModel;
using System.ComponentModel;

namespace DapperDemo
{
    public class PersonModel : BaseModel
    {
        public override  string PrimaryKey { get; } = "";
        public override string IdentityKey { get; } = "";
        public override string DbName { get; } = "";
        public override string TableName { get; } = "";
        public override string ConnectionString { get; } = ConfigHelper.Get("");

        /// <summary>
        /// 主键Id
        /// </summary>
        [Description("主键Id")]
        public int Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Description("姓名")]
        public string Name { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Description("生日")]
        public virtual DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Description("性别")]
        public bool Sex { get; set; }
    }
}
