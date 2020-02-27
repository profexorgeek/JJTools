using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Toolchest.Logging;
using Toolchest.Models;

namespace Toolchest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Change logger type here if needed
            Log.Instance = new ConsoleLogger();
            Log.Instance.Level = LogLevels.Info;

            // Call whatever methods need to be called here to get some grunt-work done!
        }
    }
}
