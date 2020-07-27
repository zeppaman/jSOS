using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JsOS.APP.Services;
using Microsoft.AspNetCore.Mvc;

namespace JsOS.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FileSystemController:BaseController
    {
        public FileSystemController(DatabaseService databaseService, MessageBusService messageBusService) : base(databaseService, messageBusService)
        { }


        [HttpPost("file/save")]
        public bool Save([FromQuery]string filepath,[FromBody] string contentBase64)
        {
            ComputePermission("file/write",HttpContext);
            System.IO.File.WriteAllBytes(filepath, Convert.FromBase64String(contentBase64));
            return true;
        }

        [HttpGet("file/read")]
        public string Read([FromQuery]string filepath)
        {
            ComputePermission("file/read", HttpContext);
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(filepath));
        }

        [HttpDelete("file/delete")]
        public void Delete([FromQuery] string filepath)
        {
            ComputePermission("file/delete", HttpContext);
            System.IO.File.Delete(filepath);
        }



        [HttpPost("directory/create")]
        public DirectoryInfo CreateDirectory([FromQuery]string path)
        {
            ComputePermission("file/write", HttpContext);
            return Directory.CreateDirectory (path);
        }

        [HttpDelete("directory/delete")]
        public void DeleteDirectory([FromQuery]string path)
        {
            ComputePermission("file/write", HttpContext);
            Directory.Delete(path);
        }

        [HttpGet("directory/files")]
        public string[] ListFiles([FromQuery]string path, [FromQuery]string searchPattern, [FromQuery]bool recursive)
        {
            
            ComputePermission("file/read",HttpContext);
            return Directory.GetFiles(path, searchPattern, new EnumerationOptions() { RecurseSubdirectories = recursive });
        }


    }
}
