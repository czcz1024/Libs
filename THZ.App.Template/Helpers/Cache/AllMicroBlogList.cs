namespace THZ.App.Template.Helpers.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using THZ.App.Template.Handlers.Events.MicroBlogAddedHandlers;
    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.DataModel;

    using Uninf.Bus;
    using Uninf.Cache;
    using Uninf.CacheData;
    using Uninf.Data;
    using Uninf.Model;

    public class AllMicroBlogList:ListBase<MicroBlogCache>
    {
        private IUnitOfWork uow;

        private IModelConverter<UserMicroBlog, MicroBlogCache> converter;

        public AllMicroBlogList(ICache cache, IUnitOfWork uow, IModelConverter<UserMicroBlog, MicroBlogCache> converter)
            : base(cache)
        {
            this.uow = uow;
            this.converter = converter;
        }

        protected override IEnumerable<MicroBlogCache> RebuildList()
        {
            var list = uow.GetRepository<UserMicroBlog>().AsQueryable().Where(x => x.VisitRole != 2);
            return list.ToList().Select(converter.Convert);
        }

        protected override long GetAllCount()
        {
            return uow.GetRepository<UserMicroBlog>().AsQueryable().Count(x => x.VisitRole != 2);
        }

        protected override Func<MicroBlogCache, long> Score()
        {
            return x =>x.AddTime.Ticks;
        }

        protected override IEnumerable<MicroBlogCache> RebuildPage(int skip, int take, bool desc, out long all)
        {
            var list = uow.GetRepository<UserMicroBlog>().AsQueryable().Where(x => x.VisitRole != 2);
            all = list.Count();
            var r = desc
                        ? list.OrderByDescending(x => x.AddDate).ThenByDescending(x => x.Id)
                        : list.OrderBy(x => x.AddDate).ThenBy(x => x.Id);
            return r.Skip(skip).Take(take).ToList().Select(converter.Convert);
        }
    }
}