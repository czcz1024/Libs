namespace THZ.App.Template.Helpers.Cache
{
    using System.Collections.Generic;
    using System.Linq;

    using THZ.App.Template.Interfaces;
    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.DataModel;

    using Uninf.Cache;
    using Uninf.CacheData;
    using Uninf.Data;
    using Uninf.Model;

    public class FriendsMicroBlog:ManyRelatedBase<MicroBlogCache,int,int,UserPubFriendBlogList>
    {
        private ITHZHelper thz;

        private IUnitOfWork uow;

        private IModelConverter<UserMicroBlog, MicroBlogCache> converter;

        public FriendsMicroBlog(ICache cache, UserPubFriendBlogList oneGetter, ITHZHelper thz, IUnitOfWork uow, IModelConverter<UserMicroBlog, MicroBlogCache> converter)
            : base(cache, oneGetter)
        {
            this.thz = thz;
            this.uow = uow;
            this.converter = converter;
        }

        protected override long GetAllCount(IEnumerable<int> keys)
        {
            return
                uow.GetRepository<UserMicroBlog>().AsQueryable().Count(x => keys.Contains(x.UserId) && x.VisitRole != 2);
        }

        protected override IEnumerable<MicroBlogCache> OrderBy(IEnumerable<MicroBlogCache> src)
        {
            return src.OrderByDescending(x => x.AddTime).ThenByDescending(x=>x.Id);
        }

        protected override IEnumerable<int> GetManyKeys(int mainKey)
        {
            return thz.UserFriends(mainKey);
        }

        public override IEnumerable<MicroBlogCache> GetRelatedPage(int key, int skip, int take, out long all, bool desc = true)
        {
            if (skip == 0)
            {
                return base.GetRelatedPage(key, skip, take, out all, desc);
            }
            var keys = GetManyKeys(key);
            var list=uow.GetRepository<UserMicroBlog>().AsQueryable().Where(x => keys.Contains(x.UserId) && x.VisitRole!=2);
            all = list.Count();

            var result = desc
                             ? list.OrderByDescending(x => x.AddDate).ThenByDescending(x => x.Id)
                             : list.OrderBy(x => x.AddDate).ThenBy(x => x.Id);
            var page = result.Skip(skip).Take(take).ToList();
            return page.Select(converter.Convert);
        }
    }
}