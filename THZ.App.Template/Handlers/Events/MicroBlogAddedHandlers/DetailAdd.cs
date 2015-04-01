namespace THZ.App.Template.Handlers.Events.MicroBlogAddedHandlers
{
    using THZ.App.Template.Config;
    using THZ.App.Template.Helpers.Cache;
    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.Events;

    using Uninf.Bus;
    using Uninf.CacheData;
    using Uninf.Config;
    using Uninf.Model;

    public class DetailAdd:HandlerBase<MicroBlogAdded>
    {
        private IObjSetter<MicroBlogCache> objsetter;
        private MyMicroBlogList setter;

        private IModelConverter<MicroBlogAdded, MicroBlogCache> converter;

        public DetailAdd(IMessageSerializer ser, IObjSetter<MicroBlogCache> objsetter, MyMicroBlogList setter, IModelConverter<MicroBlogAdded, MicroBlogCache> converter)
            : base(ser)
        {
            this.objsetter = objsetter;
            this.setter = setter;
            this.converter = converter;
        }

        public override void Handle(MicroBlogAdded msg)
        {
            //var obj = converter.Convert(msg);

            //objsetter.Set(obj);

            //setter.AddToRelated(msg.UserId, obj);
            //setter.AddToRelatedPage(msg.UserId, obj);
        }

        public override int Sort()
        {
            return THZConfigHelper<AppConfig>.Instance.QueueConfig.DetailHandler.Sort;
        }

        public override bool Async()
        {
            return THZConfigHelper<AppConfig>.Instance.QueueConfig.DetailHandler.Async;
        }
    }
}