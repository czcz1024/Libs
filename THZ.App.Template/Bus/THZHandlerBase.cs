namespace THZ.App.Template.Bus
{
    using System.Linq;

    using Uninf.Bus;
    using Uninf.Bus.RabbitMq;
    using Uninf.Bus.THZ;
    using Uninf.Cache;

    //public abstract class THZHandlerBase<T>:THZMqHandlerBase<T>
    //{
    //    protected IMessageSerializer ser;

    //    private ICache cache;

    //    protected THZHandlerBase(IMessageServerConfig config, IHandlerThreadStore db, IMessageSerializer ser, ICache cache)
    //        : base(config, db)
    //    {
    //        this.ser = ser;
    //        this.cache = cache;
    //    }

    //    public override T Deserialize(string msg)
    //    {
    //        return this.ser.Deserialize<T>(msg);
    //    }

    //    protected override bool IsSkipThisMessage(RabbitMQ.Client.IBasicProperties e, string routingKey, string msg)
    //    {
    //        //return base.IsSkipThisMessage(e, routingKey, msg);
    //        var key = "HandlerRecived:" + this.GetType().FullName;
    //        var id = e.MessageId;
    //        if (cache.ContainsKey(key))
    //        {
    //            if (cache.GetAllFromList(key).Contains(id))
    //            {
    //                return true;
    //            }
    //        }
            
    //        return false;
    //    }

    //    protected override void SaveDoneMessage(RabbitMQ.Client.IBasicProperties e, string routingKey, string message)
    //    {
    //        //base.SaveDoneMessage(basicProperties, routingKey, message);
    //        var key = "HandlerRecived:" + this.GetType().FullName;
    //        var id = e.MessageId;
    //        cache.AddToList(key, id);
    //    }
    //}
}