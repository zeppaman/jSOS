using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using JsOS.APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace JsOS.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class OSController : BaseController
    {
        public OSController() : base()
        { }


        [HttpGet("whoami")]
        public string Whoami()
        {
            ComputePermission("os/read");
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }


        [HttpGet("username")]
        public string GetUsername()
        {
            ComputePermission("os/read");
            return Environment.UserName;
        }


        [HttpGet("domainname")]
        public string GetDomainName()
        {
            ComputePermission("os/read");
            return Environment.UserDomainName;
        }


        [HttpGet("variable")]
        public string GetVariable(string variableName)
        {
            ComputePermission("os/read");
            return Environment.GetEnvironmentVariable(variableName);
        }


        [HttpGet("cpu")]
        public float GetCPUUsed()
        {
            ComputePermission("os/read");
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return cpuCounter.NextValue();
        }

        [HttpGet("ram")]
        public float GetRAMAvailable()
        {
            ComputePermission("os/read");
            PerformanceCounter ramCounter;
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }
    }
}
