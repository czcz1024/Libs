namespace THZ.App.Template.Config
{
    using System.Collections.Generic;
    using System.Configuration;

    using THZ.App.Template.Handlers.Events.MicroBlogAddedHandlers;

    using Uninf.Bus.THZ;
    using Uninf.Cache.Redis;
    using Uninf.Config;

    public class AppConfig:XmlFileConfigBase<AppConfig>
    {
        private AppConfig()
        {
            this.QueueConfig = new QueueConfig();
            this.RedisConfig = new RedisConfig(){
                ConnectionStrings=new[]{"127.0.0.1"},
                KeyPrefix="THZ-MicroBlog:"
            };
        }

        public QueueConfig QueueConfig { get; set; }

        public RedisConfig RedisConfig { get; set; }

        public override AppConfig Create()
        {
            return new AppConfig();
        }

        public override string FilePath()
        {
            return ConfigurationManager.AppSettings["configPath"] + "config.config";
        }

        public override string DefaultFilePath()
        {
            return ConfigurationManager.AppSettings["configPath"] + "default.config";
        }
    }

    public class QueueConfig : THZQueueConfig
    {
        public QueueConfig()
        {
            ConnectionString = "amqp://localhost";
            ExchangeName = "THZEx";
            RetryExchangeName = "THZRetryEx";
            DeadExchangeName = "THZDeadEx";
            DeadQueueName = "THZDeadQueue";
            ExchangeType = "direct";
            RetryTimes = 5;
            RetryWaitSecond = 5;

            DetailHandler = new HandlerConfig { 
                Name=typeof(DetailAdd).FullName,
                Sort=1,
                Async=false
            };

            PubFriendHandler = new HandlerConfig { 
                Name=typeof(UserPublicBlogs).FullName,
                Sort=1,
                Async=true,
            };
        }

        public HandlerConfig DetailHandler { get; set; }

        public HandlerConfig PubFriendHandler { get; set; }
    }

    public class HandlerConfig
    {
        public string Name { get; set; }

        public bool Async { get; set; }

        public int Sort { get; set; }
    }
}