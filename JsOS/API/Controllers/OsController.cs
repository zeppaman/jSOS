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
        public OSController(DatabaseService databaseService, MessageBusService messageBusService) : base(databaseService, messageBusService)
        { }


        [HttpGet("whoami")]
        public string Whoami()
        {
            ComputePermission("os/read", HttpContext);
            return Environment.UserDomainName + "\\" + Environment.UserName;
        }


        [HttpGet("username")]
        public string GetUsername()
        {
            ComputePermission("os/read", HttpContext);
            return Environment.UserName;
        }


        [HttpGet("domainname")]
        public string GetDomainName()
        {
            ComputePermission("os/read", HttpContext);
            return Environment.UserDomainName;
        }


        [HttpGet("variable")]
        public string GetVariable(string variableName)
        {
            ComputePermission("os/read", HttpContext);
            return Environment.GetEnvironmentVariable(variableName);
        }


        [HttpGet("cpu")]
        public float GetCPUUsed()
        {
            ComputePermission("os/read", HttpContext);
            PerformanceCounter cpuCounter;
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            return cpuCounter.NextValue();
        }

        [HttpGet("ram")]
        public float GetRAMAvailable()
        {
            ComputePermission("os/read", HttpContext);
            PerformanceCounter ramCounter;
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            return ramCounter.NextValue();
        }
    }
}
