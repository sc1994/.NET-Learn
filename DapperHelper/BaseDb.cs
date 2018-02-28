using DapperModel;
using System.Collections.Generic;

namespace DapperHelper
{
    public interface IBaseDb<TModel, TKey> where TModel : BaseModel
    {
        
    }



    public static class PersonData
    {
        public static Value Add(this Value that, string msg)
        {
            that.Ha.Add(msg);
            return that;
        }
    }

    public class Value
    {
        internal List<string> Ha { get; set; } = new List<string>();
    }
}
