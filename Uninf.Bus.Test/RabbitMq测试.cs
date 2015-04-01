using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Bus.Test
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Autofac;
    using Autofac.Extras.CommonServiceLocator;

    using Microsoft.Practices.ServiceLocation;

    using RabbitMQ.Client;

    using Uninf.Bus.RabbitMq;
    using Uninf.Bus.THZ;

    [TestClass]
    public class RabbitMq测试
    {
        //[AssemblyCleanup]
        public static void InitTest()
        {
            var config = new Config();
            var factory = new ConnectionFactory { Uri = config.GetConnectionString() };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {

                    channel.ExchangeDelete(config.ExchangeName);
                    channel.ExchangeDelete(config.DeadExchangeName);
                    channel.ExchangeDelete(config.RetryExchangeName);
                    channel.QueueDelete(config.DeadQueueName);

                    channel.QueueDelete("THZ-" + typeof(QHandler).FullName + "-Queue");
                    channel.QueueDelete("THZ-" + typeof(QHandler).FullName + "-Queue-Retry");

                    channel.QueueDelete("THZ-" + typeof(QHandler1).FullName + "-Queue");
                    channel.QueueDelete("THZ-" + typeof(QHandler1).FullName + "-Queue-Retry");

                    channel.QueueDelete("THZ-" + typeof(QHandler2).FullName + "-Queue");
                    channel.QueueDelete("THZ-" + typeof(QHandler2).FullName + "-Queue-Retry");

                }
            }
        }

        [TestMethod]
        public void rabbitmq发送接收测试()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Config>().AsImplementedInterfaces();
            builder.RegisterType<RabbitBus>().AsImplementedInterfaces();
            builder.RegisterType<HandlerThreadStore>().AsImplementedInterfaces();
            builder.RegisterType<QHandler>().AsImplementedInterfaces();
            builder.RegisterType<JsonMessageSerializer>().AsImplementedInterfaces();

            builder.RegisterType<THZListener>().AsImplementedInterfaces();

            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            var s = container.Resolve<IListener>();
            var li = ServiceLocator.Current.GetAllInstances<IHandler>().Where(x => x.Async());
            foreach (var l in li)
            {
                s.Start(l);
            }

            QHandler.TestResult = 0;
            QHandler.RunDone = false;
            var bus = ServiceLocator.Current.GetInstance<IMessageBus>();
            bus.Send(new Msg { });

            int wait = 0;
            while (true)
            {
                if (QHandler.RunDone)
                {
                    Assert.AreEqual(1, QHandler.TestResult, "值不正确");
                    break;
                }

                wait++;
                Thread.Sleep(1000);
                if (wait > 10)
                {
                    Assert.AreEqual(1, QHandler.TestResult, "超时");
                    break;
                }
            }

            //foreach (var item in HandlerThreadStore.Dic)
            //{
            //    if (item.Key == typeof(QHandler))
            //    {
            //        foreach (var i in item.Value)
            //        {
            //            i.Item2.Cancel();
            //        }
            //    }
            //}

        }

        [TestMethod]
        public void 多处理器测试()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Config>().AsImplementedInterfaces();
            builder.RegisterType<RabbitBus>().AsImplementedInterfaces();
            builder.RegisterType<HandlerThreadStore>().AsImplementedInterfaces();
            builder.RegisterType<QHandler2>().AsImplementedInterfaces();
            builder.RegisterType<QHandler1>().AsImplementedInterfaces();
            builder.RegisterType<JsonMessageSerializer>().AsImplementedInterfaces();

            builder.RegisterType<THZListener>().AsImplementedInterfaces();

            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            var s = container.Resolve<IListener>();
            var li = ServiceLocator.Current.GetAllInstances<IHandler>().Where(x => x.Async());
            foreach (var l in li)
            {
                s.Start(l);
            }

            var bus = ServiceLocator.Current.GetInstance<IMessageBus>();
            bus.Send(new Msg1 { });

            int wait = 0;
            while (true)
            {
                if (TestResults.H1Done && TestResults.H2Done)
                {
                    Assert.AreEqual(2, TestResults.Result, "值不正确");
                    break;
                }

                wait++;
                Thread.Sleep(1000);
                if (wait > 15)
                {
                    Assert.AreEqual(2, TestResults.Result, "超时");
                    break;
                }
            }

            //foreach (var item in HandlerThreadStore.Dic)
            //{
            //    if (item.Key != typeof(QHandler))
            //    {
            //        foreach (var i in item.Value)
            //        {
            //            i.Item2.Cancel();
            //        }
            //    }
            //}
        }

        [TestMethod]
        public void 重试测试()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Config>().AsImplementedInterfaces();
            builder.RegisterType<RabbitBus>().AsImplementedInterfaces();
            builder.RegisterType<HandlerThreadStore>().AsImplementedInterfaces();
            builder.RegisterType<QHandler>().AsImplementedInterfaces();
            builder.RegisterType<JsonMessageSerializer>().AsImplementedInterfaces();

            builder.RegisterType<THZListener>().AsImplementedInterfaces();

            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            var s = container.Resolve<IListener>();
            var li = ServiceLocator.Current.GetAllInstances<IHandler>().Where(x => x.Async());
            foreach (var l in li)
            {
                s.Start(l);
            }

            QHandler.ErrResult = 0;
            var bus = ServiceLocator.Current.GetInstance<IMessageBus>();
            bus.Send(new Msg { Text = "err" });

            int wait = 0;
            var cfg = new Config();
            Thread.Sleep(30000);
            Assert.AreEqual(cfg.RetryTimes + 1, QHandler.ErrResult);
            //while (true)
            //{
            //    if (QHandler.RunDone)
            //    {
            //        Assert.AreEqual(1, QHandler.TestResult,"值不正确");
            //        break;
            //    }

            //    wait++;
            //    Thread.Sleep(1000);
            //    if (wait > 10)
            //    {
            //        Assert.AreEqual(1, QHandler.TestResult, "超时");
            //        break;
            //    }
            //}
        }
    }

    public class Config : THZQueueConfig
    {
        public Config()
        {
            this.ConnectionString = "amqp://localhost";
            this.DeadExchangeName = "TestDeadEx";
            this.DeadQueueName = "TestDeadQueue";
            this.ExchangeName = "TestWorkEx";
            this.ExchangeType = "direct";
            this.RetryExchangeName = "TestRetryEx";
            this.RetryTimes = 5;
            this.RetryWaitSecond = 5;
        }
    }

    public class RabbitBus : RabbitMqBus
    {
        public RabbitBus(IRabbitServerConfig config, IMessageSerializer serializor)
            : base(config, serializor)
        {
        }

        protected override IEnumerable<IHandler<T>> GetHandlers<T>()
        {
            return ServiceLocator.Current.GetAllInstances<IHandler<T>>();
        }

        protected override IResultHandler<T, TResult> GetResultHandler<T, TResult>()
        {
            return ServiceLocator.Current.GetInstance<IResultHandler<T, TResult>>();
        }
    }

    public class TestResults
    {
        public static int Result = 0;
        public static bool H1Done = false;
        public static bool H2Done = false;
    }

    public class QHandler : HandlerBase<Msg>
    {
        public static int TestResult = 0;
        public static bool RunDone = false;

        public static int ErrResult = 0;

        public QHandler(IMessageSerializer ser)
            : base(ser)
        {
        }

        public override void Handle(Msg msg)
        {

            if (msg.Text == "err")
            {
                ErrResult += 1;
                throw new Exception("test err");
            }
            else
            {
                TestResult += 1;
                RunDone = true;
            }

        }

        public override int Sort()
        {
            return 1;
        }

        public override bool Async()
        {
            return true;
        }
    }

    public class QHandler1 : HandlerBase<Msg1>
    {
        public QHandler1(IMessageSerializer ser)
            : base(ser)
        {
        }

        public override void Handle(Msg1 msg)
        {
            TestResults.Result += 1;
            TestResults.H1Done = true;
        }

        public override int Sort()
        {
            return 1;
        }

        public override bool Async()
        {
            return true;
        }
    }

    public class QHandler2 : HandlerBase<Msg1>
    {
        public QHandler2(IMessageSerializer ser)
            : base(ser)
        {
        }

        public override void Handle(Msg1 msg)
        {
            TestResults.Result += 1;
            TestResults.H2Done = true;
        }

        public override int Sort()
        {
            return 2;
        }

        public override bool Async()
        {
            return true;
        }
    }

    public class Msg1
    {
    }
}
