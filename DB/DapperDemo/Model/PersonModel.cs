using System;
using System.ComponentModel;

namespace DapperDemo
{
    public class PersonModel: ModelBase
    {
        public static string IdentityKey { get; } = "Id";
        public static string DbName { get; } = "Person";
        public static string TableName { get; } = "TestDB";

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual DateTime Birthday { get; set; }

        public bool Sex { get; set; }

        public PersonModel() : base("Id") { }
    }

    public enum PersonEnum
    {
        [Description("主键Id")]
        Id,

        [Description("姓名")]
        Name,

        [Description("生日")]
        Birthday,

        [Description("性别")]
        Sex
    }

    public abstract class ModelBase
    {
        public static string PrimaryKey { get; private set; }

        protected ModelBase(string primaryKey)
        {
            PrimaryKey = primaryKey;
        }
    }
}
