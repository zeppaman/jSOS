using System;
using System.Collections.Generic;
using System.Text;
using JsOS.APP.Core;

namespace JsOS.APP.Model
{
    public class Need : ObservableObject
    {
        private string permission;
        private bool enabled;

        public string Permission { get => permission; set => SetAndNotify<string>(ref this.permission, value, () => this.Permission); }
        public bool Enabled { get => enabled; set => SetAndNotify<bool>(ref this.enabled, value, () => this.Enabled); }
    }
    public class AppPermission : ObservableObject
    {
        private Guid id = Guid.Empty;
        private string appName = "APP NAME";
        private string token = "APP NAME";
        private List<Need> needs = new List<Need>();


        public Guid Id { get => id; set => SetAndNotify<Guid>(ref this.id, value, () => this.Id); }
        public string AppName { get => appName; set => SetAndNotify<string>(ref this.appName, value, () => this.AppName); }
        public string Token { get => token; set => SetAndNotify<string>(ref this.token, value, () => this.Token); }
        public List<Need> Needs { get => needs; set => SetAndNotify<List<Need>>(ref this.needs, value, () => this.Needs); }
    }
}
