using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;

using System.Net;
using JsOS.API;
using Microsoft.Extensions.DependencyInjection;
using JsOS.APP.Services;
using JsOS.APP.Model;
using Microsoft.AspNetCore.Http;

namespace JsOS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static ServiceProvider ServiceProvider;
        public App()
        {
            LoadDepedencies();
            Init();
        }

        private void Init()
        {
            var config=App.ServiceProvider.GetService(typeof(ConfigService)) as ConfigService;
            config.Init();

            //TODO: move on init service

            var db = App.ServiceProvider.GetService(typeof(DatabaseService)) as DatabaseService;
            var settings=db.GetSettings();
            if (settings == null)
            {
                db.SaveSettings(new Settings());
            }


            var serverService = App.ServiceProvider.GetService(typeof(ServerService)) as ServerService;
            serverService.RestartServer(settings);
        }
        public static ServiceCollection Services { get; set; } = new ServiceCollection();
        private void LoadDepedencies()
        {


            Services.AddSingleton<ConfigService>();
            Services.AddSingleton<DatabaseService>();
            Services.AddSingleton<ServerService>();
            Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
            Services.AddSingleton<MessageBusService>();

            App.ServiceProvider = Services.BuildServiceProvider();

        }

    }
}
