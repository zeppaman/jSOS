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

        private void LoadDepedencies()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ConfigService>();
            services.AddSingleton<DatabaseService>();
            services.AddSingleton<ServerService>();

            App.ServiceProvider = services.BuildServiceProvider();

        }

    }
}
