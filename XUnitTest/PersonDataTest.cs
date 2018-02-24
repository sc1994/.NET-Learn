using System;
using System.Diagnostics;
using Xunit;
using Utilities;
using DapperDemo;

namespace XUnitTest
{
    public class PersonDataTest
    {
        private readonly int _id = 0;

        [Fact]
        public void IsExit1()
        {
            var result = PersonData.Instance.IsExist(_id);
            Assert.False(result);
        }

        [Fact]
        public void IsExit2()
        {
            var result = PersonData.Instance.IsExist(
               new Where<PersonModel>().Or(x => x.Name, RelationEnum.Equal, "123")
                                       .And(x => x.Id, RelationEnum.Greater, 12));
            Assert.False(result);
        }

        [Fact]
        public void GetList()
        {
            var result = PersonData.Instance.GetList(
                new Show<PersonModel>().Add(x => x.Name)
                                       .Add(x => x.Birthday)
                                       .Add(x => x.Sex),
                new Where<PersonModel>().And(x => x.Name, RelationEnum.In, new[] { "123", "345", "456" })
                                        .Or(x => x.Birthday, RelationEnum.Greater, DateTime.Now.AddYears(-18)),
                new Sort<PersonModel>().Asc(x => x.Name));
            Assert.Null(result);
        }


    }
}
