namespace THZ.App.Template.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.ServiceLocation;

    using THZ.App.Template.Helpers.Cache;
    using THZ.App.Template.Interfaces;
    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.Commands;
    using THZ.App.Template.Models.ViewModel;

    using Uninf.Bus;
    using Uninf.CacheData;
    using Uninf.Model;

    public class AppHelper:IAppHelper
    {
        private IMessageBus bus;

        private ITHZHelper thz;

        public AppHelper(IMessageBus bus, ITHZHelper thz)
        {
            this.bus = bus;
            this.thz = thz;
        }

        public string Test()
        {
            return "test";
        }

        public bool SameInstance(ITHZHelper thz1)
        {
            return thz == thz1;
        }

        public int Add(AddMicroBlog cmd)
        {
            return bus.Call<AddMicroBlog, int>(cmd);
        }

        public void Delete(DeleteMicroBlog cmd)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<MyMicroBlogViewModel> MyBlogs(int uid,out long all,int page=1,int pagesize=10)
        {
            var skip = (page - 1) * pagesize;
            var list = ServiceLocator.Current.GetInstance<MyMicroBlogList>().GetRelatedPage(uid, skip,pagesize, out all, true);
            var converter = ServiceLocator.Current.GetInstance<IModelConverter<MicroBlogCache, MyMicroBlogViewModel>>();
            return list.Select(converter.Convert);
        }

        public IEnumerable<UserMicroBlogViewModel> UserBlogs(int uid,int? visitor, out long all, int page = 1, int pagesize = 10)
        {
            IRelatedGetter<MicroBlogCache, int> dao = ServiceLocator.Current.GetInstance<UserPubBlogList>();
            if (visitor.HasValue && thz.UserFriends(visitor.Value).Contains(uid))
            {
                dao = ServiceLocator.Current.GetInstance<UserPubFriendBlogList>();
            }
            var list = dao.GetRelatedPage(uid, 0, 10, out all, true);
            var converter =
                ServiceLocator.Current.GetInstance<IModelConverter<MicroBlogCache, UserMicroBlogViewModel>>();
            return list.Select(converter.Convert);
        }

        public IEnumerable<FriendMicroBlogViewModel> FriendBlogs(int uid, int page, int pagesize, out long all)
        {
            var skip = (page - 1) * pagesize;
            var list = ServiceLocator.Current.GetInstance<FriendsMicroBlog>()
                .GetRelatedPage(uid, skip, pagesize, out all, true);
            var converter =
                ServiceLocator.Current.GetInstance<IModelConverter<MicroBlogCache, FriendMicroBlogViewModel>>();
            return list.Select(converter.Convert);
        }

        public IEnumerable<FriendMicroBlogViewModel> All(int page, int pagesize, out long all)
        {
            var skip = (page - 1) * pagesize;
            var list = ServiceLocator.Current.GetInstance<AllMicroBlogList>().Page(skip, pagesize, out all, true);
            var converter =
                ServiceLocator.Current.GetInstance<IModelConverter<MicroBlogCache, FriendMicroBlogViewModel>>();
            return list.Select(converter.Convert);
        }
    }
}