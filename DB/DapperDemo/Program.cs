using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using DapperHelper;
using DapperModel;
using Utilities;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //var s = Param.Projection();
            //var a = Param.Projection().Add("123").Add("123").Add("123").Add("123").Add("123").Add("123").Add("123").Add("123").Add("123");
            //var b = Param.Projection().Add("456").Add("456").Add("456").Add("456").Add("456").Add("456").Add("456").Add("456").Add("456");

            //D<A>.Instance.Add(new A());
            //D<A1>.Instance.Add(new A1());

            //Console.WriteLine(GetModelInfo(new PersonModel()));

            //var s = Definition<PersonModel>.Where().And(x => x.Id, RelationEnum.Equal, "123");
            //var a = Definition<PersonModel>.Where().And(x => x.Id, RelationEnum.Equal, "456");
            //var b = Definition<PersonModel>.Where().And(x => x.Id, RelationEnum.Equal, "789");


            //var a = Param.Value.Add("123");

            Console.ReadLine();
        }

        public static string GetModelInfo<T>(T t)
        {
            var tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            var properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (var item in properties)
            {
                var name = item.Name; //名称  
                var value = item.GetValue(t, null);  //值  
                var attribute = Attribute.GetCustomAttribute(item, typeof(DescriptionAttribute));
                if (attribute == null) continue;
                var des = ((DescriptionAttribute)attribute).Description;// 属性值  

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1}:{2},", name, value, des);
                }
                else
                {
                    GetModelInfo(value);
                }
            }
            return tStr;
        }
    }

    class Param
    {
        private static Value _value = new Value();

        public static Value Value {
            get { return _value; }
        }
    }

    class Value
    {
        private List<string> _v = new List<string>();

        public void Add(string m)
        {
            _v.Add(m);
        }
    }

    class ValueNew
    {
        public ValueNew(string m)
        {

        }
    }



}
