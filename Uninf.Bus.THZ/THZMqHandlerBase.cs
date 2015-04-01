namespace Uninf.Bus.THZ
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    using Newtonsoft.Json;

    using RabbitMQ.Client;
    using RabbitMQ.Client.Events;

    using Uninf.Bus.RabbitMq;

    //public abstract class THZMqHandlerBase<T> : RabbitMqHandlerBase<T>
    //{
    //    protected THZMqHandlerBase(IMessageServerConfig config, IHandlerThreadStore db)
    //        : base(config, db)
    //    {
    //    }

    //    protected override int RetryWaitSecond()
    //    {
    //        return config.GetRetryWaitSeconds();
    //    }

    //    protected override int RetryTimes()
    //    {
    //        return config.GetRetryTimes();
    //    }

    //    protected override bool IsSkipThisMessage(IBasicProperties e, string routingKey, string msg)
    //    {
    //        return false;
    //    }

    //    protected override void SaveDoneMessage(IBasicProperties basicProperties, string routingKey, string message)
    //    {
            
    //    }
    //}
}