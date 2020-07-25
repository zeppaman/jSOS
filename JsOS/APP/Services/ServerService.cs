using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using JsOS.API;
using JsOS.APP.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace JsOS.APP.Services
{
    public class ServerService
    {
        public ServerService()
        {
            
        }

       private  IWebHost server=null;
        public void RestartServer(Settings settings)
        {
            StopServer(settings);
            if (settings.Enabled == false)
            {
                return;
            }

             this.server = WebHost.CreateDefaultBuilder().UseKestrel(x =>
              {
                  
                  if (settings.AllowExternalIps)
                  {
                      x.ListenAnyIP(settings.PortNumber);                      
                  }

                  x.ListenLocalhost(settings.PortNumber);

              }).UseStartup<ApiStartup>().Build();
            
            Task.Run(() =>
            {
                server.Run();

            });

        }

        public void StopServer(Settings settings)
        {
            if (this.server != null)
            {
                this.server.StopAsync().Wait();
            }

            
        }
    }
}
