using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using JsOS.APP.Core;
using JsOS.APP.Model;
using JsOS.APP.Services;

namespace JsOS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DatabaseService db;
        private ServerService serverService;
        private MessageBusService messageBusService;

        bool statusToUpdate = true;
        public MainWindow()
        {


            this.db = App.ServiceProvider.GetService(typeof(DatabaseService)) as DatabaseService;
            this.serverService = App.ServiceProvider.GetService(typeof(ServerService)) as ServerService;
            this.messageBusService = App.ServiceProvider.GetService(typeof(MessageBusService)) as MessageBusService;



            this.messageBusService.RegisterEvent("appchanged", (appid) =>
            {
                Dispatcher.Invoke(() =>
                {
                    ReloadData();
                });
                
            });


            this.messageBusService.RegisterEvent("serverstatuschanged", (status) =>
            {
                ServerStatus = this.serverService.GetServerStatus();
                this.Title = "jSOS Server Status:" + ServerStatus;
                statusToUpdate = true;
            });

            SelectChildPermission = new RelayCommand<List<object>>(args =>
            {
                OnSelectChildPermission(args);
            }, o => true);



            SavePermission = new RelayCommand((obj) =>
            {
                OnSavePermission(obj);
            }, o => true);

            SaveSettings = new RelayCommand<Settings>((obj) =>
            {
                OnSaveSettings(obj);
            }, o => true);

            ReloadData();

           

            InitializeComponent();

            
        }


        private void ReloadData()
        {
            var apps = this.db.GetAppPermission().FindAll().ToList();

            apps.ForEach((x) => { this.AppPermission.Add(x); });


            this.Settings = this.db.GetSettings();
        }

        public RelayCommand<List<object>> SelectChildPermission { get; set; }

        public RelayCommand SavePermission { get; set; }

        public RelayCommand<Settings> SaveSettings { get; set; }

        private void OnSavePermission(object o)
        {
            foreach (var app in this.AppPermission)
            {
                this.db.SavePermission(app);
            }
        }

        private void OnSaveSettings(Settings o)
        {
          
            this.db.SaveSettings(this.Settings);

            Dispatcher.Invoke(() =>
            {
                this.serverService.RestartServer(this.Settings);
            });
        }

        private void OnSelectChildPermission(List<object> args)
        {
            AppPermission app = args[0] as AppPermission;
            ;
            AppPermission binded = AppPermission.Where(x => x.AppName == app.AppName).FirstOrDefault();

            var enabled = (app.Enabled.HasValue == false || app.Enabled.Value == false);



            foreach (var item in binded.Needs)
            {
                item.Enabled = enabled;
            }
        }

        private void OnSelectChildPermissionItem(List<object> args)
        {
            AppPermission app = args[0] as AppPermission;
            bool? selection = (bool?)args[1];
            AppPermission binded = AppPermission.Where(x => x.AppName == app.AppName).FirstOrDefault();

            
        }

        public BindingList<AppPermission> AppPermission
        {
            get
            {
                return (BindingList<AppPermission>)GetValue(AppPermissionProperty);
            }
            set
            {
                SetValue(AppPermissionProperty, value);
            }
        }

        public  readonly
             DependencyProperty AppPermissionProperty
                 = DependencyProperty.Register(
                                     "AppPermissionProperty",
                                     typeof(BindingList<AppPermission>),
                                     typeof(MainWindow), new UIPropertyMetadata(new BindingList<AppPermission>()));



        public string ServerStatus
        {
            get
            {
                return (string)GetValue(ServerStatusProperty);
            }
            set
            {
                SetValue(ServerStatusProperty, value);
            }
        }

        public readonly
             DependencyProperty ServerStatusProperty
                 = DependencyProperty.Register(
                                     "ServerStatusProperty",
                                     typeof(string),
                                     typeof(MainWindow), new UIPropertyMetadata("started"));





        public Settings Settings
        {
            get
            {
                return (Settings)GetValue(SettingsProperty);
            }
            set
            {
                SetValue(SettingsProperty, value);
            }
        }

        public  readonly
             DependencyProperty SettingsProperty
                 = DependencyProperty.Register(
                                     "SettingsProperty",
                                     typeof(Settings),
                                     typeof(MainWindow));


    }
}
