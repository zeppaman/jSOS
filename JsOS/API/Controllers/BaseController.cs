using System;
using System.Collections.Generic;
using System.Text;
using JsOS.APP.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace JsOS.API.Controllers
{
    public abstract class BaseController
    {

        HttpContextAccessor httpContextAccessor;
        public BaseController()
        {
            this.DatabaseService = App.ServiceProvider.GetService(typeof(DatabaseService)) as DatabaseService;
            this.httpContextAccessor = App.ServiceProvider.GetService(typeof(HttpContextAccessor)) as HttpContextAccessor;
        }

        public DatabaseService DatabaseService { get ; set ; }

        public void ComputePermission(string permission)
        {
            var token = this.httpContextAccessor.HttpContext.Request.Headers["Token"];
            var appname= this.httpContextAccessor.HttpContext.Request.Headers["AppName"];

            var app=this.DatabaseService.GetAppPermission().FindOne(x => x.AppName == appname && x.Token == token);
            if (app == null) throw new Exception("App not found");
            if (!app.Needs.Any(x=>x.Enabled&& x.Permission==permission))
            {
                throw new Exception("Not authorized");
            }
        }
    }
}
