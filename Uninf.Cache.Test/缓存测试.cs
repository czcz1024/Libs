using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack.Redis;
using Uninf.Cache.Redis;
using Uninf.CacheData;
using RedisConfig = Uninf.Cache.Redis.RedisConfig;

namespace Uninf.Cache.Test
{
    using System.Linq;

    [TestClass]
    public class 缓存测试
    {
        static readonly IRedisConfig RedisConfig = new RedisConfig { ConnectionStrings = new[] { "192.168.1.223" } };
        readonly ICache _cache = new RedisCache(RedisConfig);

        [TestMethod]
        public void Test()
        {
            var clientManager = new PooledRedisClientManager(new[] { "192.168.1.223" });
            clientManager.GetClient();
        }

        #region obj测试
        /// <summary>
        /// obj模型获取测试
        /// </summary>
        [TestMethod]
        public void BlogGet()
        {
            var blogBase = new BlogBase(_cache);
            blogBase.Get(1);
        }

        /// <summary>
        /// obj模型写入测试
        /// </summary>
        [TestMethod]
        public void BlogSet()
        {
            var blog = new Blog { Id = 1, Title = "标题", Content = "内容" };
            var blogBase = new BlogBase(_cache);
            blogBase.Set(blog);
        }

        /// <summary>
        /// obj模型批量获取
        /// </summary>
        [TestMethod]
        public void BlogGets()
        {
            var blogBase = new BlogBase(_cache);
            var ilist = new[] { 1, 2, 3 };
            blogBase.Gets(ilist);
        }

        /// <summary>
        /// obj模型删除
        /// </summary>
        [TestMethod]
        public void BlogDel()
        {
            var blogBase = new BlogBase(_cache);
            var blog = new Blog { Id = 3, Title = "标题", Content = "内容" };
            blogBase.Delete(blog);
        }
        #endregion

        #region list测试
        /// <summary>
        /// 添加模型到list测试
        /// </summary>
        [TestMethod]
        public void BlogAddToList()
        {
            var bloglist = new BlogListBase(_cache);
            var blog = new Blog { Id = 4, Title = "标题", Content = "内容" };
            bloglist.AddToList(blog);
        }

        /// <summary>
        /// 从list删除模型测试
        /// </summary>
        [TestMethod]
        public void BlogDelFromList()
        {
            var bloglist = new BlogListBase(_cache);
            var blog = new Blog { Id = 5, Title = "标题", Content = "内容" };
            bloglist.RemoveFromList(blog);
        }

        /// <summary>
        /// 获取列表测试
        /// </summary>
        [TestMethod]
        public void BlogGetList()
        {
            var bloglist = new BlogListBase(_cache);
            bloglist.GetList();
        }

        //[TestMethod]
        //public void BlogAddToPage()
        //{
        //    var bloglist = new BlogListGetter(_cache);
        //    var blog = new Blog { Id = 4, Title = "标题", Content = "内容" };
        //    bloglist.AddToPage(blog);
        //}

        //[TestMethod]
        //public void BlogRemoveFromPage()
        //{
        //    var bloglist = new BlogListGetter(_cache);
        //    var blog = new Blog { Id = 4, Title = "标题", Content = "内容" };
        //    bloglist.RemoveFromPage(blog);
        //}

        //[TestMethod]
        //public void BlogPage()
        //{
        //    var bloglist = new BlogListGetter(_cache);
        //    long l;
        //    bloglist.Page(1, 1, out l);
        //}
        #endregion

        #region related测试
        /// <summary>
        /// related添加元素
        /// </summary>
        [TestMethod]
        public void BlogAddToRelated()
        {
            var blogrelated = new BlogRelatedBase(_cache);
            var blog = new Blog { Id = 1, Title = "标题", Content = "内容" };
            blogrelated.AddToRelated("1", blog);
        }

        /// <summary>
        /// 获取related
        /// </summary>
        [TestMethod]
        public void BlogGetRelated()
        {
            var blogrelated = new BlogRelatedBase(_cache);
            blogrelated.GetRelated("1");
        }

        /// <summary>
        /// 删除related元素
        /// </summary>
        [TestMethod]
        public void BlogRemoveFromRelated()
        {
            var blogrelated = new BlogRelatedBase(_cache);
            var blog = new Blog { Id = 5, Title = "标题", Content = "内容" };
            blogrelated.RemoveFromRelated("1", blog);
        }

        [TestMethod]
        public void BlogAddToRelatedPage()
        {
            var blogrelated = new BlogRelatedBase(_cache);
            var blog = new Blog { Id = 5, Title = "标题", Content = "内容" };
            blogrelated.AddToRelatedPage("1", blog);
        }

        [TestMethod]
        public void BlogRemoveFromRelatedPage()
        {
            var blogrelated = new BlogRelatedBase(_cache);
            var blog = new Blog { Id = 5, Title = "标题", Content = "内容" };
            blogrelated.RemoveFromRelatedPage("1", blog);
        }

        [TestMethod]
        public void BlogGetRelatedPage()
        {
            var blogrelated = new BlogRelatedBase(_cache);
            long l;
            blogrelated.GetRelatedPage("1", 1, 1, out l);
        }
        #endregion

        #region 微博-评论测试
        /// <summary>
        /// 添加related微博-评论
        /// </summary>
        [TestMethod]
        public void CurrentRelated()
        {
            var currentrelated = new CurrentBlogRelatedBase(_cache);
            var current = new Current { Id = 1, CurrentDetail = "评论内容" };
            currentrelated.AddToRelated(1, current);
        }

        [TestMethod]
        public void CurrentRelated2()
        {
            var currentrelated = new CurrentBlogRelatedBase2(_cache);
            var current = new Current { Id = 4, CurrentDetail = "评论内容" };
            currentrelated.AddToRelated(3, current);
        }
        #endregion

        #region manyrelated测试
        //[TestMethod]
        //public void BlogManyGetRelated()
        //{
        //    var blogrelated = new BlogRelatedBase(_cache);
        //    var blogMany = new BlogManyRelated(_cache, blogrelated);
        //    blogMany.GetRelated("1");
        //}

        //[TestMethod]
        //public void BlogManyGetRelatedPage()
        //{
        //    var blogrelated = new BlogRelatedBase(_cache);
        //    var blogMany = new BlogManyRelated(_cache, blogrelated);
        //    long l;
        //    blogMany.GetRelatedPage("1", 1, 1, out l);
        //}

        //[TestMethod]
        //public void BlogManyGetChildren()
        //{
        //    var blogrelated = new BlogRelatedBase(_cache);
        //    var blogMany = new BlogManyRelated(_cache, blogrelated);
        //    var slist = new List<string> { "1", "2", "3" };
        //    long l;
        //    blogMany.GetChildren(slist, 1, 1, out l);
        //}
        #endregion

    }

    #region 模型（测试用）
    public class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Pwd { get; set; }

    }

    public class Current
    {
        public int Id { get; set; }

        public string CurrentDetail { get; set; }

    }
    #endregion

    public class BlogBase : ObjBase<Blog, int>
    {
        public BlogBase(ICache cache)
            : base(cache)
        {
        }

        protected override Blog Rebuild(int id)
        {
            var blog = new Blog { Id = id, Title = "标题", Content = "内容" };
            return blog;
        }

        protected override Dictionary<int, Blog> Rebuild(params int[] keys)
        {
            var dictionary = new Dictionary<int, Blog>();
            foreach (var key in keys)
            {
                var blog = new Blog { Id = key, Title = "标题", Content = "内容" };
                dictionary.Add(key, blog);
            }
            return dictionary;
        }
    }

    public class BlogListBase : ListBase<Blog>
    {
        public BlogListBase(ICache cache)
            : base(cache)
        {
        }

        protected override IEnumerable<Blog> RebuildList()
        {
            return GetList();
        }

        protected override long GetAllCount()
        {
            throw new NotImplementedException();
        }

        protected override Func<Blog, long> Score()
        {
            return a => a.Id;
        }

        protected override IEnumerable<Blog> RebuildPage(int skip, int take, bool desc, out long all)
        {
            all = GetList().Count();
            return GetList();
        }

    }

    public class BlogRelatedBase : RelatedBase<User, Blog, string>
    {
        public BlogRelatedBase(ICache cache)
            : base(cache)
        {
        }

        protected override IEnumerable<Blog> Rebuild(string key)
        {
            var bloglist = new List<Blog>();
            var blog = new Blog { Id = 1, Title = "1", Content = "1" };
            bloglist.Add(blog);
            return bloglist;
        }

        protected override IEnumerable<Blog> RebuildPage(string key, int skip, int take, bool desc, out long all)
        {
            all = GetRelated(key).Count();
            return GetRelated(key);
        }

        protected override long GetAllCount(string pid)
        {
            throw new NotImplementedException();
        }

        protected override string RelatedName()
        {
            return "User_Blog";
        }

        protected override Func<Blog, long> Score()
        {
            return a => a.Id;
        }
    }

    public class CurrentBlogRelatedBase : RelatedBase<Blog, Current, int>
    {
        public CurrentBlogRelatedBase(ICache cache)
            : base(cache)
        {
        }

        protected override IEnumerable<Current> Rebuild(int key)
        {
            return GetRelated(key);
        }

        protected override IEnumerable<Current> RebuildPage(int key, int skip, int take, bool desc, out long all)
        {
            all = GetRelated(key).Count();
            return GetRelated(key);
        }

        protected override long GetAllCount(int pid)
        {
            throw new NotImplementedException();
        }

        protected override string RelatedName()
        {
            return "Blog_Current";
        }

        protected override Func<Current, long> Score()
        {
            return a => a.Id;
        }
    }

    public class CurrentBlogRelatedBase2 : RelatedBase<Blog, Current, int>
    {
        public CurrentBlogRelatedBase2(ICache cache)
            : base(cache)
        {
        }

        protected override IEnumerable<Current> Rebuild(int key)
        {
            return GetRelated(key);
        }

        protected override IEnumerable<Current> RebuildPage(int key, int skip, int take, bool desc, out long all)
        {
            all = GetRelated(key).Count();
            return GetRelated(key);
        }

        protected override long GetAllCount(int pid)
        {
            throw new NotImplementedException();
        }

        protected override string RelatedName()
        {
            return "Blog_Current";
        }

        protected override Func<Current, long> Score()
        {
            return a => a.Id;
        }
    }

    //public class BlogManyRelated : ManyRelatedBase<Blog, string, string, BlogRelatedBase>
    //{
    //    public BlogManyRelated(ICache cache, BlogRelatedBase oneGetter)
    //        : base(cache, oneGetter)
    //    {
    //    }

    //    protected override IEnumerable<Blog> OrderBy(IEnumerable<Blog> src)
    //    {
    //        return GetRelated("1");
    //    }

    //    protected override IEnumerable<string> GetManyKeys(string mainKey)
    //    {
    //        var slist = new List<string>();
    //        return slist;
    //    }
    //}
}