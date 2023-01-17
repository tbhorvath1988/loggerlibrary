using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerClassLib
{
    internal class InvalidLoggerTypeException : Exception
    {
        public InvalidLoggerTypeException(string message) : base(message) { }

    }
}
