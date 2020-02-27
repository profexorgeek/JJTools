using System;
using System.Collections.Generic;
using System.Text;

namespace Toolchest.Logging
{
    interface ILogger
    {
        public LogLevels Level { get; set; }

        public void Debug(string msg);

        public void Info(string msg);

        public void Warn(string msg);

        public void Error(string msg);
    }
}
