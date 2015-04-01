namespace THZ.App.Template.Handlers.Events.MicroBlogAddedHandlers
{
    using System;
    using System.Threading;

    using THZ.App.Template.Config;
    using THZ.App.Template.Helpers.Cache;
    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.Events;

    using Uninf.Bus;
    using Uninf.Cache;
    using Uninf.Config;
    using Uninf.Model;

    public class UserPublicBlogs : HandlerBase<MicroBlogAdded>
    {
        private UserPubBlogList pub;

        private UserPubFriendBlogList pubfriend;

        private IModelConverter<MicroBlogAdded, MicroBlogCache> converter;

        private ICache cache;

        public UserPublicBlogs(IMessageSerializer ser, UserPubBlogList pub, UserPubFriendBlogList pubfriend, IModelConverter<MicroBlogAdded, MicroBlogCache> converter, ICache cache)
            : base(ser)
        {
            this.pub = pub;
            this.pubfriend = pubfriend;
            this.converter = converter;
            this.cache = cache;
        }

        public override void Handle(MicroBlogAdded msg)
        {
            Thread.Sleep(10000);
            var rnd = new Random();
            if (rnd.Next(9) > 5)
            {
                cache.Increment("testv");
            }
            else
            {
                throw new Exception();
            }
            //var obj = converter.Convert(msg);
            //if (msg.VisitRole == 0)
            //{
            //    pub.AddToRelated(msg.UserId, obj);
            //    pub.AddToRelatedPage(msg.UserId, obj);

            //    pubfriend.AddToRelated(msg.UserId, obj);
            //    pubfriend.AddToRelatedPage(msg.UserId, obj);
                
            //}
            //if (msg.VisitRole ==1)
            //{
            //    pubfriend.AddToRelated(msg.UserId, obj);
            //    pubfriend.AddToRelatedPage(msg.UserId, obj);
            //}
        }

        public override int Sort()
        {
            return THZConfigHelper<AppConfig>.Instance.QueueConfig.PubFriendHandler.Sort;
        }

        public override bool Async()
        {
            return THZConfigHelper<AppConfig>.Instance.QueueConfig.PubFriendHandler.Async;
        }
    }
}