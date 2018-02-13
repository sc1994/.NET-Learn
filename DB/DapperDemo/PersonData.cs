using DapperExtensions;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DapperDemo
{
    class PersonData : IBaseDb<PersonModel, PersonEnum, int>
    {
        public bool IsExist(int key)
        {
            using (var cn = new SqlConnection(""))
            {
                var predicate = Predicates.Field<PersonModel>(f => f.Id, Operator.Eq, key);
                return cn.Count<PersonModel>(predicate) > 0;
            }
        }

        public bool IsExist(params IBasePredicate[] wheres)
        {
            throw new System.NotImplementedException();
        }

        public long Count(params IBasePredicate[] wheres)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(PersonModel model, bool log = false)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Dictionary<PersonEnum, object> update, IBasePredicate[] wheres, int top = 0)
        {
            throw new System.NotImplementedException();
        }

        public int Insert(PersonModel model)
        {
            throw new System.NotImplementedException();
        }

        public void InsertMany(IList<PersonModel> models)
        {
            throw new System.NotImplementedException();
        }

        public PersonModel Get(int key)
        {
            throw new System.NotImplementedException();
        }

        public PersonModel Get(params IBasePredicate[] wheres)
        {
            throw new System.NotImplementedException();
        }

        public PersonModel Get(IBasePredicate[] wheres, params PersonEnum[] shows)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetList(params IBasePredicate[] wheres)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetList(IBasePredicate[] wheres, params PersonEnum[] shows)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetPage(PersonEnum[] shows, IBasePredicate[] wheres, PersonEnum orders, int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetPage()
        {
            throw new System.NotImplementedException();
        }
    }
}
