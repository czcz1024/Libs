using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace THZ.App.Template.Controllers.Api
{
    using Microsoft.Practices.ServiceLocation;

    using THZ.App.Template.Interfaces;

    public class AppApiController : ApiController
    {
        private IAppHelper helper;

        private ITHZHelper thz;

        public AppApiController(IAppHelper helper, ITHZHelper thz)
        {
            this.helper = helper;
            this.thz = thz;
        }

        [HttpGet]
        public string Get()
        {
            return helper.SameInstance(thz).ToString();
        }
    }
}
