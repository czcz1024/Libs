namespace THZ.App.Template.Helpers.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ServiceStack.Text;

    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.DataModel;
    using THZ.App.Template.Models.ServiceModel;
    

    using Uninf.Cache;
    using Uninf.CacheData;
    using Uninf.Data;
    using Uninf.Model;

    public class MyMicroBlogList:RelatedBase<THZUserInfo,MicroBlogCache,int>
    {
        private IUnitOfWork uow;

        private IModelConverter<UserMicroBlog, MicroBlogCache> converter;

        public MyMicroBlogList(ICache cache, IUnitOfWork uow, IModelConverter<UserMicroBlog, MicroBlogCache> converter)
            : base(cache)
        {
            this.uow = uow;
            this.converter = converter;
        }

        protected override IEnumerable<MicroBlogCache> Rebuild(int key)
        {
            return
                uow.GetRepository<UserMicroBlog>()
                    .AsQueryable()
                    .Where(x => x.UserId == key).ToList()
                    .Select(x => converter.Convert(x))
                    .ToList();
        }

        protected override IEnumerable<MicroBlogCache> RebuildPage(int key, int skip, int take, bool desc,out long all)
        {
            var src = uow.GetRepository<UserMicroBlog>().AsQueryable().Where(x => x.UserId == key);
            all = src.Count();
            var list = desc ? src.OrderByDescending(x => x.AddDate).ThenByDescending(x=>x.Id) : src.OrderBy(x => x.AddDate).ThenBy(x=>x.Id);
            return list.Skip(skip).Take(take).ToList().Select(converter.Convert);
        }

        protected override long GetAllCount(int pid)
        {
            var cacheKey = getCacheKey(pid);
            if (!cache.ContainsKey(cacheKey))
            {
                var cnts= uow.GetRepository<UserMicroBlog>().AsQueryable().Count(x => x.UserId == pid);
                cache.Set(cacheKey, cnts);
                return cnts;
            }
            return cache.Get<int>(cacheKey);
        }

        protected override string RelatedName()
        {
            return "MyMicroBlogList";
        }

        private string getCacheKey(int pid)
        {
            return "cnt:" + this.GetType().Name + ":" + pid;
        }

        protected override void AfterAddToPage(int pid, bool always, bool hasDo)
        {
            if (hasDo)
            {
                cache.Increment(getCacheKey(pid), 1);
            }
        }

        protected override void AfterDeleteFromPage(int pid, int DeleteCnt)
        {
            cache.Decrement(getCacheKey(pid), (uint)DeleteCnt);
        }

        protected override Func<MicroBlogCache, long> Score()
        {
            return x => x.Id;
        }
    }
}