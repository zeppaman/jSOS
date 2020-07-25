using System;
using System.Collections.Generic;
using System.Text;
using JsOS.APP.Core;
using System.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;
using LiteDB;

namespace JsOS.APP.Model
{
    public class Need : ObservableObject
    {
        private string permission;
        private bool enabled;

        public string Permission { get => permission; set => SetAndNotify<string>(ref this.permission, value, () => this.Permission); }
        public bool Enabled { get => enabled; 
            set => SetAndNotify<bool>(ref this.enabled, value, () => this.Enabled); }
    }
    public class AppPermission : ObservableObject
    {
        private Guid id = Guid.Empty;
        private string appName = "APP NAME";
        private string token = "APP NAME";
        private BindingList<Need> needs = new BindingList<Need>();

        private bool? enabled = false;
      


        public Guid Id { get => id; set => SetAndNotify<Guid>(ref this.id, value, () => this.Id); }
        public string AppName { get => appName; set => SetAndNotify<string>(ref this.appName, value, () => this.AppName); }
        public string Token { get => token; set => SetAndNotify<string>(ref this.token, value, () => this.Token); }
        public BindingList<Need> Needs { get => needs;
            set {
                SetAndNotify<BindingList<Need>>(ref this.needs, value, () => this.Needs);
                this.needs.ListChanged += Needs_ListChanged;
                UpdateEnabled();
            } 
        }

        [BsonIgnore]
        public bool? Enabled { get => enabled; set => SetAndNotify<bool?>(ref this.enabled, value, () => this.Enabled); }

        private void Needs_ListChanged(object sender, ListChangedEventArgs e)
        {
            SetAndNotify<BindingList<Need>>(ref this.needs, this.needs, () => this.Needs);

            UpdateEnabled();

        }

        private void UpdateEnabled()
        {
            var allOn = this.needs.Count(x => x.Enabled == true);
            var allOff = this.needs.Count(x => x.Enabled == false);
            var all = this.needs.Count;
            if (all == allOff) Enabled = false;
            else if (all == allOn) Enabled = true;
            else Enabled = null;
        }
    }
}
