using System;
using System.ComponentModel;

namespace DapperDemo
{
    public class PersonModel 
    {
        [Description("主键Id")]
        public int Id { get; set; }

        [Description("姓名")]
        public string Name { get; set; }

        [Description("生日")]
        public virtual DateTime Birthday { get; set; }

        [Description("性别")]
        public bool Sex { get; set; }
    }
}
