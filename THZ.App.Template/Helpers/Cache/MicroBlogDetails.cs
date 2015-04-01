namespace THZ.App.Template.Helpers.Cache
{
    using System;
    using System.Linq.Expressions;

    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.DataModel;

    using Uninf.Cache;
    using Uninf.Data;
    using Uninf.Model;
    using System.Linq;

    public class MicroBlogDetails : ObjDetailBase<UserMicroBlog,MicroBlogCache, int>
    {
        public MicroBlogDetails(ICache cache, IUnitOfWork uow, IModelConverter<UserMicroBlog, MicroBlogCache> converter)
            : base(cache, uow, converter)
        {
        }

        protected override Expression<Func<UserMicroBlog, bool>> GetWhere(params int[] keys)
        {
            return x => keys.Contains(x.Id);
        }

        protected override Func<UserMicroBlog, int> KeySelector()
        {
            return x => x.Id;
        }
    }
}