using System;
using System.ComponentModel;
using System.Reflection;
using DapperHelper;
using Utilities;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = Param.Projection();
            var a = Param.Projection().Add("123").Add("123").Add("123").Add("123").Add("123").Add("123").Add("123").Add("123").Add("123");
            var b = Param.Projection().Add("456").Add("456").Add("456").Add("456").Add("456").Add("456").Add("456").Add("456").Add("456");

            D<A>.Instance.Add(new A());
            D<A1>.Instance.Add(new A1());

            Console.WriteLine(GetModelInfo(new PersonModel()));

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
        public static Value Projection()
        {
            return new Value();
        }
    }


    class A : B
    {
        public override string Id { get; } = "123";
    }

    class A1 : B
    {
        public override string Id { get; } = "456";
    }

    abstract class B
    {
        public abstract string Id { get; }
    }

    interface IC<in T> where T : B
    {
        void Add(T model);
    }

    class D<T> : IC<T> where T : B, new()
    {
        public static D<T> Instance => new Lazy<D<T>>(() => new D<T>()).Value;

        private static readonly T Model = new Lazy<T>(() => new T()).Value;

        public void Add(T model)
        {
            Console.WriteLine(Model.Id);
        }
    }
}
