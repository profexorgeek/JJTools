using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Toolchest.Logging;
using Toolchest.Models;

namespace Toolchest
{
    class FolderSizeScanner
    {
        string startPath;
        string outputCsv;

        public FolderSizeScanner(string startPath, string outputCsv) {

            this.startPath = startPath;
            this.outputCsv = outputCsv;
        }

        public void Scan()
        {
            if(!Directory.Exists(startPath))
            {
                throw new Exception($"Provided path doesn't exist: ${startPath}");
            }

            using (var writer = new StreamWriter(outputCsv, false))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader(typeof(DirectorySizeData));
                writer.WriteLine();
            }

            GetDirSizeDataRecursive(startPath);
        }

        private DirectorySizeData GetDirSizeDataRecursive(string path)
        {
            Log.Instance.Info($"Getting directory size for: {path}");

            string[] files;
            string[] directories;
            var dir = new DirectorySizeData
            {
                Path = path,
                DirectoryBytes = 0,
                DirectoryFileCount = 0,
                DirectoryDirectoryCount = 0,
                Notes = ""
            };

            try
            {
                files = Directory.GetFiles(path);
                directories = Directory.GetDirectories(path);
            }
            // EARLY OUT: can't scan this path!
            catch(Exception e)
            {
                Log.Instance.Error(e.Message);
                dir.Notes = e.Message;
                return dir;
            }

            dir.DirectoryFileCount = files.Length;
            dir.DirectoryDirectoryCount = directories.Length;

            foreach (var f in files)
            {
                dir.DirectoryBytes += new FileInfo(f).Length;
            }

            foreach (var d in directories)
            {
                if(new DirectoryInfo(d).Name.StartsWith("."))
                {
                    continue;
                }

                var subdir = GetDirSizeDataRecursive(d);
                dir.TotalTreeBytes += subdir.TotalTreeBytes;
            }

            using (var writer = new StreamWriter(outputCsv, true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecord(dir);
                writer.WriteLine();
            }

            return dir;
        }
    }
}
