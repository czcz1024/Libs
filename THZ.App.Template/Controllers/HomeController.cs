using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THZ.App.Template.Controllers
{
    using THZ.App.Template.Interfaces;

    public class HomeController : Controller
    {
        private IAppHelper helper;

        public HomeController(IAppHelper helper)
        {
            this.helper = helper;
        }

        public ActionResult Index()
        {
            var result = helper.Test();
            return View("","",result);
        }
    }
}