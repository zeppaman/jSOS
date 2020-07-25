using System;
using System.Collections.Generic;
using System.Text;
using JsOS.APP.Model;
using LiteDB;
using System.Linq;

namespace JsOS.APP.Services
{
    public class DatabaseService
    {

        private  LiteDatabase db;
        private ConfigService configService;

        private ILiteCollection<AppPermission> AppPermissionCollection;

        private ILiteCollection<Permission> PermissionCollection;

        private ILiteCollection<Settings> SettingsCollection;

       
        public DatabaseService(ConfigService configService)
        {
            this.configService = configService;
            
            db = new LiteDatabase(this.configService.GetAppPath("data.db"));
                // Get customer collection
            this.PermissionCollection    = db.GetCollection<Permission>("Permission");
            this.AppPermissionCollection = db.GetCollection<AppPermission>("AppPermission");
            this.SettingsCollection = db.GetCollection<Settings>("Settings");

        }

        public ILiteCollection<Permission> GetPermission()
        {
            return this.PermissionCollection;
        }


        public ILiteCollection<AppPermission> GetAppPermission()
        {
            return this.AppPermissionCollection;
        }

        public void SavePermission(AppPermission app)
        {
            if (Guid.Empty.Equals(app.Id))
            {
                app.Id = Guid.NewGuid();
                this.AppPermissionCollection.Insert(app);
            }
            else
            {
                this.AppPermissionCollection.Update(app);
            }
        }


        public Settings GetSettings()
        {
            return this.SettingsCollection.FindAll().FirstOrDefault();
        }

        public void SaveSettings(Settings settings)
        {
            var saved = GetSettings();
            if (saved == null)
            {
                settings.Id = Guid.NewGuid();

                this.SettingsCollection.Insert(settings);
            }
            else
            {
                settings.Id = saved.Id; //caller cannot change id
                this.SettingsCollection.Update(settings);
            }

        }
    }
}
