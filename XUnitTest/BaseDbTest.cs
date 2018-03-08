using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using DapperModel;
using DapperHelper;

namespace XUnitTest
{
    public class BaseDbTest
    {
        private int _testCount;
        private readonly IBaseDb<Person> _provider = BaseDb<Person>.Instance;

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
                });
            Assert.True(tuple.Result.result); // 插入

            var model = _provider.GetByIdentityKey(tuple.Result.identityKey);
            Assert.Equal(model.Name, $"suncheng_{_testCount}"); // 查询

            var result = _provider.DeleteAsync(model.IdNumber); // 删除
            Assert.True(result.Result);
        }

        [Fact]
        public void InsertRange_DeleteRange()
        {
            var modelList = new List<Person>
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
                                    Sex = 1,
                                    Phone = "1370000000001"
                                },
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
                                    Sex = 1,
                                    Phone = "1370000000001"
                                },
                                new Person
                                {
                                    Name = $"suncheng_{++_testCount}",
                                    Birthday = DateTime.Now.AddDays(_testCount),
                                    Sex = 1,
                                    Phone = "1370000000001"
                                }
                            };

            var insert = _provider.InsertRangeAsync(modelList);
            Assert.True(insert.Result == modelList.Count);

            var delete = _provider.DeleteRangeAsync(Where<Person>.Init().And(x => x.IdNumber, RelationEnum.In, modelList.Select(x => x.IdNumber)));

            Assert.True(delete.Result == modelList.Count);
        }
    }
}
