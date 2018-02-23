using System;
using System.ComponentModel;

namespace DapperDemo
{
    public class PersonModel : BaseModel
    {
        public PersonModel() : base("Id", "Id", "Person","TestDB")
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual DateTime Birthday { get; set; }

        public bool Sex { get; set; }
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


    public class BaseModel
    {
        public string PrimaryKey { get; }
        public string IdentityKey { get; }
        public string DbName { get; }
        public string TableName { get; }

        public BaseModel(string primaryKey, string identityKey, string dbName, string tableName)
        {
            PrimaryKey = primaryKey;
            IdentityKey = identityKey;
            DbName = dbName;
            TableName = tableName;
        }
    }
}
