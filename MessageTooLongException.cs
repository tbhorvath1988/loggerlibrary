using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerClassLib
{
    internal class MessageTooLongException : Exception
    {
        public MessageTooLongException(string message) : base(message) { }

    }
}
