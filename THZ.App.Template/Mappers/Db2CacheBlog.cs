namespace THZ.App.Template.Mappers
{
    using System;

    using AutoMapper;

    using THZ.App.Template.Models.CacheModel;
    using THZ.App.Template.Models.Commands;
    using THZ.App.Template.Models.DataModel;
    using THZ.App.Template.Models.Events;
    using THZ.App.Template.Models.ViewModel;

    using Uninf.Model.Automapper;

    public class Db2CacheBlog:AutoMapperConverter<UserMicroBlog,MicroBlogCache>
    {
        public override void Regist()
        {
            //base.Regist();
            Mapper.CreateMap<UserMicroBlog, MicroBlogCache>()
                .ForMember(x => x.AddTime, cfg => cfg.MapFrom(src => src.AddDate))
                .ForMember(x => x.Body, cfg => cfg.MapFrom(src => src.BlogContent))
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(src => src.UserId));
        }
    }

    public class Cache2My : AutoMapperConverter<MicroBlogCache, MyMicroBlogViewModel>
    {
        public override void Regist()
        {
            Mapper.CreateMap<MicroBlogCache, MyMicroBlogViewModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(src => src.UserId))
                .ForMember(x => x.Body, cfg => cfg.MapFrom(src => src.Body));
        }
    }

    public class Cache2User : AutoMapperConverter<MicroBlogCache, UserMicroBlogViewModel>
    {
        public override void Regist()
        {
            Mapper.CreateMap<MicroBlogCache, UserMicroBlogViewModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(src => src.UserId))
                .ForMember(x => x.Body, cfg => cfg.MapFrom(src => src.Body));
        }
    }

    public class Cmd2DbBlog : AutoMapperConverter<AddMicroBlog, UserMicroBlog>
    {
        public override void Regist()
        {
            //base.Regist();
            Mapper.CreateMap<AddMicroBlog, UserMicroBlog>()
                .ForMember(x => x.AddDate, cfg => cfg.UseValue(DateTime.Now))
                .ForMember(x => x.BlogContent, cfg => cfg.MapFrom(src => src.Body));
        }
    }

    public class Db2EvtBlog : AutoMapperConverter<UserMicroBlog, MicroBlogAdded>
    {
        
    }

    public class Evt2CacheBlog : AutoMapperConverter<MicroBlogAdded, MicroBlogCache>
    {
        public override void Regist()
        {
            Mapper.CreateMap<MicroBlogAdded, MicroBlogCache>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(src => src.UserId))
                .ForMember(x => x.AddTime, cfg => cfg.MapFrom(src => src.AddDate))
                .ForMember(x => x.Body, cfg => cfg.MapFrom(src => src.BlogContent))
                .ForMember(x => x.VisitType, cfg => cfg.MapFrom(src => src.VisitRole));
        }
    }

    public class Cache2Friend : AutoMapperConverter<MicroBlogCache, FriendMicroBlogViewModel>
    {
        public override void Regist()
        {
            Mapper.CreateMap<MicroBlogCache,FriendMicroBlogViewModel>()
                .ForMember(x => x.Id, cfg => cfg.MapFrom(src => src.Id))
                .ForMember(x => x.UserId, cfg => cfg.MapFrom(src => src.UserId))
                .ForMember(x => x.Body, cfg => cfg.MapFrom(src => src.Body));
        }
    }
}