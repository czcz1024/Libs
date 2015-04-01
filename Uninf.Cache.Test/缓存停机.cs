using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Cache.Test
{
    using Uninf.Cache.Redis;

    [TestClass]
    public class 缓存停机
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cache = new RedisCache(new RedisConfig { 
                ConnectionStrings=new[]{"127.0.0.1"},
                DbIndex=0,
                KeyPrefix="Test-Key",
                OnSetError= (k, e) => { 
                    throw new Exception(k + "," + e.Message); } 
            });
            cache.SaveToPage<TestObj>(x => x.Id, new TestObj { Id = 1 });
        }
    }

    public class TestObj
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
