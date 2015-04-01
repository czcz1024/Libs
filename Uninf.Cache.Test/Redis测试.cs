using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Cache.Test
{
    using System.Threading;
    using System.Transactions;

    using ServiceStack.Redis;

    [TestClass]
    public class Redis测试
    {
        [TestMethod]
        public void 扫描键()
        {
            var clientsManager = new PooledRedisClientManager(new[] { "127.0.0.1" });
            var redis = clientsManager.GetClient();

            //redis.Set("obj:Blog:1-1", "a");
            //redis.Set("obj:Blog:2-1", "b");
            //redis.Set("obj:Blog:3-1", "c");

            //redis.Set("obj:Blog:4-2", "aa");
            //redis.Set("obj:Blog:5-2", "bb");
            //redis.Set("obj:Blog:6-2", "cc");

            //redis.Set("obj:Blog:7-2", "aaa");
            //redis.Set("obj:Blog:8-2", "bbb");
            //redis.Set("obj:Blog:9-2", "ccc");

            //var list = redis.ScanAllKeys("obj:Blog:*-[1,2]");

            //redis.AddItemToSet("allblog", "1-1");
            //redis.AddItemToSet("allblog", "2-1");
            //redis.AddItemToSet("allblog", "3-1");
            //redis.AddItemToSet("allblog", "4-2");
            //redis.AddItemToSet("allblog", "5-2");
            //redis.AddItemToSet("allblog", "6-2");
            //redis.AddItemToSet("allblog", "7-3");
            //redis.AddItemToSet("allblog", "8-3");
            //redis.AddItemToSet("allblog", "9-3");

            //var list = redis.ScanAllSetItems("allblog", "*-[1,2]");

            //redis.AddItemToSortedSet("sortblog", "1-1");
            //redis.AddItemToSortedSet("sortblog", "2-1");
            //redis.AddItemToSortedSet("sortblog", "3-1");
            //redis.AddItemToSortedSet("sortblog", "4-2");
            //redis.AddItemToSortedSet("sortblog", "5-2");
            //redis.AddItemToSortedSet("sortblog", "6-2");
            //redis.AddItemToSortedSet("sortblog", "7-3");
            //redis.AddItemToSortedSet("sortblog", "8-3");
            //redis.AddItemToSortedSet("sortblog", "9-3");

            //var list = redis.ScanAllSortedSetItems("sortblog", "*-[1,2]");

            //var set = redis.As<BlogScan>().SortedSets["obgBlog"];

            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 1, Title = "a", UserId = 1, Visit = 0, Type = 1, AddDate = DateTime.Now });
            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 2, Title = "b", UserId = 1, Visit = 0, Type = 2, AddDate = DateTime.Now });
            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 3, Title = "c", UserId = 2, Visit = 1, Type = 1, AddDate = DateTime.Now });
            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 4, Title = "d", UserId = 2, Visit = 2, Type = 2, AddDate = DateTime.Now });
            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 5, Title = "e", UserId = 3, Visit = 1, Type = 1, AddDate = DateTime.Now });
            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 5, Title = "e", UserId = 11, Visit = 0, Type = 1, AddDate = DateTime.Now });
            //redis.As<BlogScan>().AddItemToSortedSet(set, new BlogScan { Id = 5, Title = "e", UserId = 12, Visit = 0, Type = 1, AddDate = DateTime.Now });

            //var list = redis.ScanAllSortedSetItems("obgBlog", "*\"UserId\":[1][11][12],*");
        }

        [TestMethod]
        public void 事务测试()
        {
            var clientsManager = new PooledRedisClientManager(new[] { "127.0.0.1" });
            var redis = clientsManager.GetClient();

            var ts=redis.CreateTransaction();
            //redis.Set("a", 1);
            ts.QueueCommand(x => redis.Set("a", 1));
            ts.Commit();
            ts.Dispose();

            //using (var ts1 = new TransactionScope())
            //{
            //    redis.Set("b", 2);
            //}
        }

        [TestMethod]
        public void 多节点()
        {
            var clientsManager = new PooledRedisClientManager(new[] { "127.0.0.1:9001", "127.0.0.1:9002" })
            {
            };
            clientsManager.OnFailover.Add(Fail);
            var redis = clientsManager.GetClient();
            redis.Set("a", 1);
            redis.Set("b", 2);
            redis.Set("c", 3);
            redis.Set("d", 4);

            
            
        }

        [TestMethod]
        public void 批量Count()
        {
            var clientsManager = new PooledRedisClientManager(new[] { "127.0.0.1" });
            var redis = clientsManager.GetClient();
            redis.Set("a", 1);
            redis.Set("b", 2);
            var list = redis.GetAll<int>(new[] { "a", "b", "c" });
        }

        private void Fail(IRedisClientsManager cm)
        {
            throw new Exception("");
        }
    }

    public class BlogScan
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int UserId { get; set; }

        public int Visit { get; set; }

        public int Type { get; set; }

        public DateTime AddDate { get; set; }
    }
}
