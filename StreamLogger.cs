using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoggerClassLib
{
 
    public class StreamLogger : Logger
    {

        private readonly string filePath = @"C:\Temp\streamlog.txt";
        SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public StreamLogger(MessageLevelOptions levelFilter) : base(levelFilter)
        {
            this.levelFilter = levelFilter;
        }
        // If the level of the message is valid, it is logged to a stream.
        protected override void Log(string message, MessageLevelOptions messageLevel)
        {
            if (CheckLevelFilter(messageLevel)) 
            {
                using (StreamWriter sr = File.AppendText(filePath))
                {
                    sr.WriteLine(FormatText(message, messageLevel));
                    sr.Close();
                }
             }
        }

        // Similar to the method Log(), but here logging doesn't block the thread.
        // Semaphors are used to take care of thread safety.
        protected override async Task LogAsync(string message, MessageLevelOptions messageLevel)
        {
            await Task.Run(async () =>
            {
                if (CheckLevelFilter(messageLevel))
                {
                    await semaphore.WaitAsync();
                    using (StreamWriter sr = File.AppendText(filePath))
                    {
                        sr.WriteLine(FormatText(message, messageLevel));
                        sr.Close();
                    }
                    semaphore.Release();
                }
            });
        }
    }
}
