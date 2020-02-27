using System;
using System.Collections.Generic;
using System.Text;

namespace Toolchest.Logging
{
    public class ConsoleLog : ILogger
    {
        public static ConsoleLog instance;
        public static ConsoleLog Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ConsoleLog();
                }
                return instance;
            }
        }

        public LogLevels Level { get; set; }

        private ConsoleLog() { }

        private void Write(LogLevels msgLevel, string message)
        {
            if(msgLevel >= Level)
            {
                Console.Write($"{msgLevel} - {message}");
            }
        }

        public void Debug(string msg)
        {
            Write(LogLevels.Debug, msg);
        }

        public void Info(string msg)
        {
            Write(LogLevels.Info, msg);
        }

        public void Warn(string msg)
        {
            Write(LogLevels.Warn, msg);
        }

        public void Error(string msg)
        {
            Write(LogLevels.Error, msg);
        }
    }
}
