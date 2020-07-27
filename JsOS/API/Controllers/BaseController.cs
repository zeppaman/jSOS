using System;
using System.Collections.Generic;
using System.Text;
using JsOS.APP.Services;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace JsOS.API.Controllers
{
    public abstract class BaseController:Controller
    {
        public BaseController(DatabaseService databaseService, MessageBusService messageBusService)
        {
            this.DatabaseService = databaseService;
            this.MessageBusService = messageBusService;

        }

        public DatabaseService DatabaseService { get ; set ; }
        public MessageBusService MessageBusService { get; set; }


        public void ComputePermission(string permission,HttpContext httpContext)
        {
            var token = httpContext.Request.Headers["Token"].ToList().FirstOrDefault();
            var appname= httpContext.Request.Headers["AppName"].ToList().FirstOrDefault();

            var app=this.DatabaseService.GetAppPermission().FindOne(x =>  appname.Equals(x.AppName,StringComparison.InvariantCultureIgnoreCase) 
            && x.Token.Equals(token, StringComparison.InvariantCultureIgnoreCase));
            if (app == null) throw new Exception("App not found");
            if (!app.Needs.Any(x=>x.Enabled&& x.Permission.Equals(permission,StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception("Not authorized");
            }
        }
    }
}
