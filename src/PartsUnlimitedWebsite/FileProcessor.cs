
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PartsUnlimited
{
    public class FileProcessor
    {
        private IHostingEnvironment hostEnv;
        private string appRootFolder;
        public FileProcessor(IHostingEnvironment Env)
        {
            
            hostEnv = Env;
            appRootFolder = hostEnv.ContentRootPath;
        }
        public async void SaveJsonToAppFolder(string appVirtualFolderPath, string fileName, string jsonContent)
        {
            var pathToFile = appRootFolder + appVirtualFolderPath.Replace("/", Path.DirectorySeparatorChar.ToString())
            + fileName;

            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(jsonContent);
            }

        }
        public async Task SaveAwaitableJsonToAppFolder(string appVirtualFolderPath, string fileName, string jsonContent)
        {
            var pathToFile = appRootFolder + appVirtualFolderPath.Replace("/", Path.DirectorySeparatorChar.ToString())
            + fileName;

            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(jsonContent);
            }

        }

        public string LoadJsonFromAppFolder(string appVirtualFolderPath, string fileName)
        {
            string jsonContent;
            var pathToFile = appRootFolder + appVirtualFolderPath.Replace("/", Path.DirectorySeparatorChar.ToString())
            + fileName;

            using (StreamReader r = File.OpenText(pathToFile)) 
            {
                jsonContent= r.ReadToEnd();
            }
            return jsonContent;
        }
    }
}
