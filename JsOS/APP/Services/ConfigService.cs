using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JsOS.APP.Services
{
    public class ConfigService
    {
        public void Init()
        {
            Directory.CreateDirectory(GetAppPath());
        }
        public string GetAppPath(string filename = "")
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GetAppName(), filename);
        }

        private string GetAppName()
        {
            return "JSOS";
        }
    }
}
