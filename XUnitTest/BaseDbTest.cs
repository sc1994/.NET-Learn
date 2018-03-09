using System;
using Xunit;
using System.Linq;
using DapperModel;
using DapperHelper;
using System.Collections.Generic;

namespace XUnitTest
{
    public class BaseDbTest
    {
        private static int _testCount;
        private readonly IBaseDb<Person> _provider = BaseDb<Person>.Instance;
        private readonly List<Person> _modelListst = new List<Person>
                                 {
                                     new Person
                                     {
                                         Name = $"suncheng_{++_testCount}",
                                         Birthday = DateTime.Now.AddDays(_testCount),
                                         Sex = 1,
                                         Phone = "1370000000001"
                                     },
                                     new Person
                                     {
                                         Name = $"suncheng_{++_testCount}",
                                         Birthday = DateTime.Now.AddDays(_testCount),
                                         Sex = 0,
                                         Phone = "1370000000001"
                                     },
                                     new Person
                                     {
                                         Name = $"suncheng_{++_testCount}",
                                         Birthday = DateTime.Now.AddDays(_testCount),
                                         Sex = 1,
                                         Phone = "1370000000011"
                                     },
                                     new Person
                                     {
                                         Name = $"suncheng_{++_testCount}",
                                         Birthday = DateTime.Now.AddDays(_testCount),
                                         Sex = 0,
                                         Phone = "1370000000001"
                                     },
                                     new Person
                                     {
                                         Name = $"suncheng_{++_testCount}",
                                         Birthday = DateTime.Now.AddDays(_testCount),
                                         Sex = 0,
                                         Phone = "1370000000011"
                                     }
                                 };

        [Fact]
        public void InsertAsync_GetByIdentityKey_DeleteAsync()
        {
            _testCount++;
            var tuple = _provider.InsertAsync(
                new Person
                {
                    Name = $"suncheng_{_testCount}",
                    Birthday = DateTime.Now.AddDays(_testCount),
                    Sex = 1,
                    Address = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa",
                    Phone = "1370000000001"
                }, true);
            Assert.True(tuple.Result.result); // 插入

            var model = _provider.GetByIdentityKey(tuple.Result.identityKey);
            Assert.Equal(model.Name, $"suncheng_{_testCount}"); // 查询

            var result = _provider.DeleteAsync(model.IdNumber, true); // 删除
            Assert.True(result.Result);
        }

        [Fact]
        public void InsertRange()
        {
            var insert = _provider.InsertRangeAsync(_modelListst, true);
            Assert.True(insert.Result == _modelListst.Count);
        }

        [Fact]
        public void DeleteRange()
        {
            var where = Where<Person>.Instance
                                     .And(x => x.Sex, RelationEnum.Equal, 0);

            var list = _provider.GetRange(where);

            var delete = _provider.DeleteRangeAsync(where, true);

            Assert.True(delete.Result == list.Count());
        }

    }
}
