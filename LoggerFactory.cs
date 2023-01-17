using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LoggerClassLib.Logger;

namespace LoggerClassLib
{
    public class LoggerFactory
    {
        
        public LoggerTypeOptions loggerType;
        // A method accepting a loggerType parameter, which decides what type of object will be returned
        public static Logger CreateLoggerOfType(LoggerTypeOptions loggerType, MessageLevelOptions levelFilter)
        {
            if (loggerType == LoggerTypeOptions.Console)
            {
                return new ConsoleLogger(levelFilter);
            }
            else if (loggerType == LoggerTypeOptions.File)
            {
                return new FileLogger(levelFilter);
            }
            else if (loggerType == LoggerTypeOptions.Stream)
            {
                return new StreamLogger(levelFilter);
            }
            else
            {
                throw (new InvalidLoggerTypeException($"InvalidLoggerTypeException: logger type should be one of the following: console, file, stream"));
            }
        }
    }
}
