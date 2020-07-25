using System;
using System.Collections.Generic;
using System.Text;
using JsOS.APP.Core;

namespace JsOS.APP.Model
{
    public  class Settings:ObservableObject
    {
        private Guid id = Guid.Empty;
        private bool enabled = true;
        private bool allowExternalIps = false;
        private int  portNumber=54320;
        private int portNumberSSL = 54321;


        public Guid Id { get => id; set => SetAndNotify<Guid>(ref this.id, value, () => this.Id); }
        public bool Enabled { get => enabled; 
            set => SetAndNotify<bool>(ref this.enabled, value, () => this.Enabled); }
        public bool AllowExternalIps { get => allowExternalIps;
            set => SetAndNotify<bool>(ref this.allowExternalIps, value, () => this.AllowExternalIps); }
        public int PortNumber { get => portNumber; set => SetAndNotify<int>(ref this.portNumber, value, () => this.PortNumber); }
        public int PortNumberSSL { get => portNumberSSL; set => SetAndNotify<int>(ref this.portNumberSSL, value, () => this.PortNumberSSL); }
    }
}
