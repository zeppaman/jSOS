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
        public FileSystemController() : base()
        { }


        [HttpPost("file/save")]
        public bool Save(string filepath, string contentBase64)
        {
            ComputePermission("file/write");
            File.WriteAllBytes(filepath, Convert.FromBase64String(contentBase64));
            return true;
        }

        [HttpPost("file/read")]
        public string Read(string filepath)
        {
            ComputePermission("file/read");
            return Convert.ToBase64String(File.ReadAllBytes(filepath));
        }

        [HttpPost("file/delete")]
        public void Delete(string filepath)
        {
            ComputePermission("file/delete");
             File.Delete(filepath);
        }



        [HttpPost("directory/create")]
        public DirectoryInfo CreateDirectory(string path)
        {
            ComputePermission("file/write");
            return Directory.CreateDirectory (path);
        }

        [HttpPost("directory/delete")]
        public void DeleteDirectory(string path)
        {
            ComputePermission("file/write");
            Directory.Delete(path);
        }

        [HttpPost("directory/files")]
        public string[] ListFiles(string path,string searchPattern,bool recursive)
        {
            ComputePermission("file/read");
            return Directory.GetFiles(path, searchPattern, new EnumerationOptions() { RecurseSubdirectories = recursive });
        }


    }
}
