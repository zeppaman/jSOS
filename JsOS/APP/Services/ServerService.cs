using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JsOS.API;
using JsOS.APP.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace JsOS.APP.Services
{
    public class ServerService
    {
        public MessageBusService messageBusService;
        public ServerService(MessageBusService messageBusService)
        {
            this.messageBusService = messageBusService;
        }

        private string serverStatus;

        private IWebHost server = null;
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

             }).UseStartup<ApiStartup>().UseDefaultServiceProvider((b,o)=> { 
                
             })
             .Build();

            serverStatus = "Starting";


            this.messageBusService.Emit("serverstatuschanged", serverStatus);
            
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                server.RunAsync();
                serverStatus = "Started";
                this.messageBusService.Emit("serverstatuschanged", serverStatus);

            });

           
           
        }

        public void StopServer(Settings settings)
        {
            if (this.server != null)
            {
                serverStatus = "Shutting down";
                this.messageBusService.Emit("serverstatuschanged", serverStatus);
                this.server.StopAsync().Wait(); 
             
            }
            Thread.Sleep(3000);
            serverStatus = "Down";
            this.messageBusService.Emit("serverstatuschanged", serverStatus);

        }

        public string GetServerStatus()
        {
            return serverStatus;
        }

    }
}
