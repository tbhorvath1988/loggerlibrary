using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerClassLib
{
 
    public class ConsoleLogger : Logger
    {
        private readonly static int longestAllowedMessage = 1000;
        SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public ConsoleLogger (MessageLevelOptions levelFilter) : base (levelFilter)
        {
            this.levelFilter = levelFilter;
        }
        
        // If the length and level of the message is valid, it is logged to the console in the proper color.
        protected override void Log(string message, MessageLevelOptions messageLevel)
        {
            CheckMessageLength(message);
            if (CheckLevelFilter(messageLevel))
            {
                SetMessageColor(messageLevel);
                Console.WriteLine(FormatText(message, messageLevel));
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        
        // Similar to the method Log(), but here logging doesn't block the thread.
        // Semaphors are used to take care of thread safety.
        protected override async Task LogAsync(string message, MessageLevelOptions messageLevel)
        {
            CheckMessageLength(message);
            await Task.Run(async () =>
             {
                await semaphore.WaitAsync();
                if (CheckLevelFilter(messageLevel))
                {
                    SetMessageColor(messageLevel);
                    Console.WriteLine(FormatText(message, messageLevel));
                    Console.ForegroundColor = ConsoleColor.White;
                 }
                semaphore.Release();
             });
        }

        // Sets the color of the message based on its level
        private static void SetMessageColor (MessageLevelOptions messageLevel)
        {
            if (messageLevel == MessageLevelOptions.Debug)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if (messageLevel == MessageLevelOptions.Info)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if (messageLevel == MessageLevelOptions.Error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }

        // Checks if the message is longer than allowed.
        private static void CheckMessageLength(string message)
        {
            if (message.Length > longestAllowedMessage)
            {
                throw (new MessageTooLongException($"MessageTooLongException: A log message should contain less than {longestAllowedMessage} characters"));
            }
        }
    }
}
