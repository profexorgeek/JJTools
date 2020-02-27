using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Toolchest
{
    // This class just gets a flat list of all files in a repository
    // it ignores directories that are typically unwanted
    public class DirectoryCrawler
    {
        private static readonly List<string> IgnorePaths = new List<string>
        {
            ".",
            "..",
            ".git",
            ".vs",
            "bin",
            "obj",
        };

        public DirectoryCrawler()
        {
        }

        public List<string> GetFilesRecursively(string path, string extension = null)
        {
            if(!Directory.Exists(path))
            {
                throw new Exception($"Invalid directory: " + path);
            }

            List<string> foundFiles = new List<string>();

            var files = Directory.GetFiles(path);
            foreach(var file in files)
            {
                if(extension != null)
                {
                    if(Path.GetExtension(file) == extension)
                    {
                        foundFiles.Add(file);
                    }
                }
                else
                {
                    foundFiles.Add(file);
                }
            }

            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                if(!IgnorePaths.Contains(dir))
                {
                    foundFiles.AddRange(GetFilesRecursively(dir, extension));
                }
            }

            return foundFiles;
        }



    }
}
