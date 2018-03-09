using System;
using System.Diagnostics;
using System.Reflection;
using System.ComponentModel;


namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            m1("m111");

            Console.ReadLine();
        }

        static void m1(string m11)
        {
            m2("m222");
        }

        static void m2(string m22)
        {
            m3("m333");
        }

        static void m3(string m33)
        {
            ResponseWrite("ResponseWrite111");
        }
        static void ResponseWrite(string ResponseWrite11, string ResponseWriteError22 = null, string ResponseWriteError33 = null)
        {
            ResponseWriteError("ResponseWriteError111");
        }

        static person ResponseWriteError(string ResponseWriteError11, string ResponseWriteError22 = null, string ResponseWriteError33 = null)
        {
            //将错误信息写入日志
            Console.WriteLine(GetStackTraceModelName());
            return null;
        }

        static string GetStackTraceModelName()
        {
            var st = new StackTrace();
            var sfs = st.GetFrames();
            var fullName = string.Empty;
            for (var i = 1; i < sfs.Length - 1; ++i)
            {
                if (StackFrame.OFFSET_UNKNOWN == sfs[i].GetILOffset()) break;
                fullName = $"{sfs[i].GetMethod()} --> {fullName}";
            }
            return fullName.TrimEnd('-', '>');
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

    class person
    {
        public DateTime Min = DateTime.MinValue;
    }
}
