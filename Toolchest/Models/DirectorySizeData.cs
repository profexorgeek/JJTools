using System;
using System.Collections.Generic;
using System.Text;

namespace Toolchest.Models
{
    public class DirectorySizeData
    {
        public string Path { get; set; }
        public float DirectoryBytes { get; set; }
        public float DirectoryMegabytes
        {
            get
            {
                return DirectoryBytes / 1024f / 1024f;
            }
        }
        public float DirectoryFileCount { get; set; }
        public float DirectoryDirectoryCount { get; set; }

        public float TotalTreeBytes { get; set; }

        public float TotalTreeMegabytes
        {
            get
            {
                return TotalTreeBytes / 1024f / 1024f;
            }
        }

        public string Notes { get; set; }
    }
}
