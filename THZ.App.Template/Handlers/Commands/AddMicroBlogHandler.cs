namespace THZ.App.Template.Handlers.Commands
{
    using System.Transactions;

    using THZ.App.Template.Models.Commands;
    using THZ.App.Template.Models.DataModel;
    using THZ.App.Template.Models.Events;

    using Uninf.Bus;
    using Uninf.Data;
    using Uninf.Model;

    public class AddMicroBlogHandler:ResultHandlerBase<AddMicroBlog,int>
    {
        private IUnitOfWork uow;

        private IMessageBus bus;

        private IModelConverter<AddMicroBlog, UserMicroBlog> dbc;

        private IModelConverter<UserMicroBlog, MicroBlogAdded> evtc;

        public AddMicroBlogHandler(IUnitOfWork uow, IMessageBus bus, IModelConverter<AddMicroBlog, UserMicroBlog> dbc, IModelConverter<UserMicroBlog, MicroBlogAdded> evtc)
        {
            this.uow = uow;
            this.bus = bus;
            this.dbc = dbc;
            this.evtc = evtc;
        }

        public override int Handle(AddMicroBlog msg)
        {
            var dbobj = dbc.Convert(msg);
            uow.GetRepository<UserMicroBlog>().Add(dbobj);

            using (var ts = new TransactionScope())
            {
                uow.SaveChanges();
                var evt = evtc.Convert(dbobj);
                for (var i = 0; i < 20; i++)
                {
                    bus.Send(evt);
                }
                ts.Complete();
            }
            return dbobj.Id;
        }
    }
}