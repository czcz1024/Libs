using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Uninf.Bus.Test
{
    using System.Collections.Generic;

    using Autofac;
    using Autofac.Extras.CommonServiceLocator;

    using Microsoft.Practices.ServiceLocation;

    [TestClass]
    public class 基本测试
    {
        [TestInitialize]
        public void Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MsgResultHandler>().AsImplementedInterfaces();
            builder.RegisterType<MsgHanlder1>().AsImplementedInterfaces();
            builder.RegisterType<MsgHanlder2>().AsImplementedInterfaces();

            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }

        [TestMethod]
        public void 调用测试()
        {
            var msg = new Msg { Id = 1, Text = "a" };
            var bus = new TestBus();
            var result = bus.Call<Msg, int>(msg);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void 群发测试()
        {
            var msg = new Msg { Id = 1, Text = "a" };
            var bus = new TestBus();
            bus.Send(msg);
            Assert.AreEqual("a,1,2", msg.Text);
        }
    }

    public class TestBus:MessageBusBase
    {
        protected override void SendToQueue<T>(T msg)
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

    public class Msg
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }

    public class MsgResultHandler : ResultHandlerBase<Msg, int>
    {
        public override int Handle(Msg msg)
        {
            return msg.Id + 1;
        }
        
    }

    public class MsgHanlder1 : HandlerBase<Msg>
    {
        public MsgHanlder1(IMessageSerializer ser)
            : base(ser)
        {
        }

        public override void Handle(Msg msg)
        {
             msg.Text += ",1";
        }

        public override int Sort()
        {
            return 1;
        }

        public override bool Async()
        {
            return false;
        }
    }

    public class MsgHanlder2 : HandlerBase<Msg>
    {
        public MsgHanlder2(IMessageSerializer ser)
            : base(ser)
        {
        }

        public override void Handle(Msg msg)
        {
            msg.Text += ",2";
        }

        public override int Sort()
        {
            return 2;
        }

        public override bool Async()
        {
            return false;
        }
    }
}
