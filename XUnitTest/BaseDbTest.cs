using System;
using System.Collections.Generic;
using DapperDemo;
using DapperHelper;
using Xunit;

namespace XUnitTest
{
    public class BaseDbTest
    {
        [Fact]
        public void InsertAsync()
        {
            var model = new Table1
            {
                CreateTime = DateTime.Now,
                TID = Guid.NewGuid(),
                beck = "suncheng"
            };
            var result = BaseDb<Table1>.Instance.InsertAsync(model);
            Assert.Equal(15, result.Result.identityKey);
        }

        [Fact]
        public void InsertRangeAsync()
        {
            var model = new List<Table1>
                        {
                            new Table1
                            {
                                CreateTime = DateTime.Now,
                                TID = Guid.NewGuid(),
                                beck = "suncheng"
                            },
                            new Table1
                            {
                                CreateTime = DateTime.Now,
                                TID = Guid.NewGuid(),
                                beck = "suncheng2"
                            },
                            new Table1
                            {
                                CreateTime = DateTime.Now,
                                TID = Guid.NewGuid(),
                                beck = "suncheng3"
                            },
                            new Table1
                            {
                                CreateTime = DateTime.Now,
                                TID = Guid.NewGuid(),
                                beck = "suncheng4"
                            }
                        };
            var result = BaseDb<Table1>.Instance.InsertRangeAsync(model);
            Assert.NotEqual(0, result.Result);
        }
    }
}
