using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THZ.App.Template.Controllers
{
    using System.Threading;

    using THZ.App.Template.Config;

    using Uninf.Bus;
    using Uninf.Bus.RabbitMq;
    using Uninf.Config;

    public class BusConfigController : Controller
    {
        // GET: BusConfig

        private IHandlerThreadStore db;

        private IListener lis;

        public BusConfigController(IListener lis, IHandlerThreadStore db)
        {
            this.lis = lis;
            this.db = db;
        }

        public ActionResult Index()
        {
            var c = THZConfigHelper<AppConfig>.Instance.QueueConfig;

            var handlers=c.GetType().GetProperties().Where(x => x.PropertyType == typeof(HandlerConfig));

            var dic = db.GetAll();

            foreach (var item in handlers)
            {
                var v = item.GetValue(c) as HandlerConfig;
                var t = Type.GetType(v.Name);
                if (!dic.ContainsKey(t))
                {
                    dic[t] = new List<Tuple<int, object>>();
                }
            }

            return View(dic);
        }

        public ActionResult Add(string name)
        {
            var t = Type.GetType(name);
            var obj = DependencyResolver.Current.GetService(t);
            var h = obj as IHandler;
            lis.Start(h);
            return RedirectToAction("index");
        }

        public ActionResult Del(string name, int id)
        {
            var t = Type.GetType(name);
            lis.Stop(t, id);
            return RedirectToAction("index");
        }

        public ActionResult DelAll(string name)
        {
            lis.StopAll(Type.GetType(name));
            return RedirectToAction("index");
        }

        public ActionResult A2N(string name)
        {
            var t = Type.GetType(name);
            var obj = DependencyResolver.Current.GetService(t) as IHandler;
            lis.DeleteAsyncHandler(obj);
            return RedirectToAction("index");
        }
    }
}