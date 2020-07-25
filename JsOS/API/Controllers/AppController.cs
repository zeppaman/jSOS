using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using JsOS.APP.Model;
using JsOS.APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace JsOS.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AppController:BaseController
    {

        public AppController() : base()
        { }

        [HttpPost("register")]
        public bool Register(AppPermissionRequest request)
        {
            var appCandidate=this.DatabaseService.GetAppPermission().FindOne((x) => x.AppName == request.AppName);
            if (appCandidate != null && appCandidate.Token != request.Token)
            {
                throw new Exception("Bad change request");
            }
             
            var appToSave = GetAppToSave(request);
            //an update request reset all permission (also the already given)
            this.DatabaseService.SavePermission(appToSave);

            if (!request.Async)
            {
                var msg = $"Allow app {appToSave.AppName} to access permission:";                

                foreach (var item in appToSave.Needs)
                {
                    msg += " " + item.Permission;
                }

                if (MessageBox.Show(msg,"Permission request",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    foreach (var item in appToSave.Needs)
                    {
                        item.Enabled = true;
                    }
                    this.DatabaseService.SavePermission(appToSave);
                    return true;
                }

            }

            return false;
        }

        private static AppPermission GetAppToSave(AppPermissionRequest request)
        {
            var appToSave = new AppPermission();
            appToSave.AppName = request.AppName;
            request.Needs.ForEach(x => appToSave.Needs.Add(new Need() { Permission = x, Enabled = false }));
            appToSave.Token = request.Token;
            return appToSave;
        }
    }
}
