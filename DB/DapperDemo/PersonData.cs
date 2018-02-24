using System;
using Utilities;
using System.Collections.Generic;

namespace DapperDemo
{
    public class PersonData : IBaseDb<PersonModel, PersonEnum, int>
    {
        private static readonly Lazy<PersonData> Lazy = new Lazy<PersonData>(() => new PersonData());

        public static PersonData Instance => Lazy.Value;

        public bool IsExist(int key)
        {
            return false;
        }

        public bool IsExist(Where<PersonModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public long Count(Where<PersonModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(PersonModel model, bool log = false)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(Dictionary<PersonEnum, object> update, Where<PersonModel> wheres, int top = 0)
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

        public PersonModel Get(Where<PersonModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public PersonModel Get(Show<PersonModel> shows, Where<PersonModel> wheres)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetList(Where<PersonModel> wheres, Sort<PersonModel> orders = null)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetList(Show<PersonModel> shows, Where<PersonModel> wheres, Sort<PersonModel> orders = null)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetPage(Show<PersonModel> shows, Where<PersonModel> wheres, Sort<PersonModel> orders, int pageIndex, int pageSize)
        {
            throw new System.NotImplementedException();
        }

        public IList<PersonModel> GetPage()
        {
            throw new System.NotImplementedException();
        }
    }
}
