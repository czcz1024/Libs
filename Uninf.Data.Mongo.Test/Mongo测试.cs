using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Data.Mongo.Test
{
    using System.Linq;

    [TestClass]
    public class Mongo测试
    {
        [TestMethod]
        public void 测试()
        {
            var client = new MongoDao(new Config());
            client.Remove<TestObj>(x => true);


            client.Add(new TestObj { Name="a"});

            var list = client.GetQueryableCollection<TestObj>().ToList();
            Assert.AreEqual(1, list.Count);
        }
    }

    public class TestObj:MongoEntityBase
    {
        public string Name { get; set; }
    }

    public class Config : MongoConfig
    {
        public Config()
        {
            this.ConnectionString = "mongodb://192.168.1.215";
            this.Database = "UninfTestDb";
        }
    }
}
