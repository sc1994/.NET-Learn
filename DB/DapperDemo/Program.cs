using System.Collections.Generic;
using System.Data.SqlClient;
using DapperExtensions;

namespace DapperDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection cn = new SqlConnection(""))
            {
                var predicate = 
                    Predicates.Field<PersonModel>(f => f.Sex, Operator.Eq, true);
                Predicates.Between<PersonModel>(x=>x.Birthday,new BetweenValues())

            }
        }
    }

    //class Person
    //{
    //    public string Active;
    //}
}
