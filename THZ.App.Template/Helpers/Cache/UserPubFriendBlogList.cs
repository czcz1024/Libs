namespace THZ.App.Template.Helpers.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.DataModel;
    using THZ.App.Template.Models.ServiceModel;
    using THZ.App.Template.Utility;

    using Uninf.Cache;
    using Uninf.CacheData;
    using Uninf.Data;
    using Uninf.Model;

    public class UserPubFriendBlogList : RelatedBase<THZUserInfo, MicroBlogCache, int>
    {
        private IUnitOfWork uow;

        private IModelConverter<UserMicroBlog, MicroBlogCache> converter;

        public UserPubFriendBlogList(ICache cache, IUnitOfWork uow, IModelConverter<UserMicroBlog, MicroBlogCache> converter)
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
                    .Where(x => x.UserId == key && (x.VisitRole == 0 || x.VisitRole==1))
                    .ToList()
                    .Select(converter.Convert);
        }

        protected override IEnumerable<MicroBlogCache> RebuildPage(int key, int skip, int take, bool desc,out long all)
        {
            var src = uow.GetRepository<UserMicroBlog>().AsQueryable().Where(x => x.UserId == key && (x.VisitRole == 0 || x.VisitRole==1));
            all = src.Count();
            var list = desc ? src.OrderByDescending(x => x.AddDate).ThenByDescending(x=>x.Id) : src.OrderBy(x => x.AddDate).ThenBy(x=>x.Id);
            return list.Skip(skip).Take(take).ToList().Select(converter.Convert);
        }

        protected override long GetAllCount(int pid)
        {
            return
                uow.GetRepository<UserMicroBlog>()
                    .AsQueryable()
                    .Count(x => x.UserId == pid && (x.VisitRole == 0 || x.VisitRole == 1));
        }

        protected override string RelatedName()
        {
            return "UserPubFriendBlogList";
        }

        protected override Func<MicroBlogCache, long> Score()
        {
            return x => x.Id;
        }
    }
}