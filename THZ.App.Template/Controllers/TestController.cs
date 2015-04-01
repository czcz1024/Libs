using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace THZ.App.Template.Controllers
{
    using THZ.App.Template.Interfaces;
    using THZ.App.Template.Models.Commands;

    public class TestController : Controller
    {
        private IAppHelper helper;

        public TestController(IAppHelper helper)
        {
            this.helper = helper;
        }

        // GET: Test
        public ActionResult Index(int page=1)
        {
            var uid = 2217;
            long all;
            var list = helper.MyBlogs(uid,out all,page,5);
            ViewBag.All = all;
            return View(list);
        }

        public ActionResult Blogs(int id)
        {
            long all;
            var uid = 2217;
            var list = helper.UserBlogs(id,uid,out all);
            ViewBag.All = all;
            return View(list);
        }

        public ActionResult Friend(int page=1)
        {
            var uid = 2217;
            long all;
            var list = helper.FriendBlogs(uid, page, 5, out all);
            ViewBag.All = all;
            return View(list);
        }

        public ActionResult All(int page = 1)
        {
            long all;
            var list = helper.All(page, 5, out all);
            ViewBag.All = all;
            return View(list);
        }

        public ActionResult Add()
        {
            var cmd = new AddMicroBlog { 
                UserId=2217,
                VisitRole=0,
                Body=DateTime.Now.ToString()
            };
            return View(cmd);
        }

        [HttpPost]
        public ActionResult Add(AddMicroBlog cmd)
        {
            var id=helper.Add(cmd);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var cmd = new DeleteMicroBlog { BlogId=id };
            helper.Delete(cmd);
            return View();
        }
    }
}