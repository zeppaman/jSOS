using System;
using System.Collections.Generic;
using System.Text;

namespace JsOS.APP.Model
{
    public class AppPermissionRequest
    {
        public string AppName { get; set; } = "APP NAME";

        public string Token { get; set; } = "APP NAME";

        public List<string> Needs { get; set; } = new List<string>();

        public bool Async { get; set; } = true;
    }
}
