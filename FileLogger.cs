using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LoggerClassLib
{
    public class FileLogger : Logger
    {
        private readonly static int maxFileSize = 5*1024;
        private readonly static string fileName = "log.txt";
        private readonly static string fileLocation = @"C:\Temp\";
        private readonly string filePath = fileLocation + fileName;
        private int archiveNumber = 1;
        SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public FileLogger(MessageLevelOptions levelFilter) : base(levelFilter)
        {
            this.levelFilter = levelFilter;
        }

        // If the level of the message is valid, it is logged to the console in the proper color.
        // If the size of the file exceeds the limit, a new archived text file is created.
        protected override void Log(string message, MessageLevelOptions messageLevel)
        {
            if (CheckLevelFilter(messageLevel))
            {
                if (File.Exists(filePath))
                {
                    FileInfo fi = new(filePath);
                    if (fi.Length >= maxFileSize)
                    {
                        File.Move(filePath, fileLocation + GetArchivedFileName(fileName));
                    }
                }
                File.AppendAllText((filePath), FormatText(message, messageLevel) + Environment.NewLine);
            }
        }

        // Similar to the method Log(), but here logging doesn't block the thread.
        // Semaphors are used to take care of thread safety.
        protected override async Task LogAsync(string message, MessageLevelOptions messageLevel)
        {
            await Task.Run(async () =>
            {
                await semaphore.WaitAsync();
                if (CheckLevelFilter(messageLevel))
                {
                    if (File.Exists(filePath))
                    {
                        FileInfo fi = new(filePath);
                        if (fi.Length >= maxFileSize)
                        {
                            File.Move(filePath, fileLocation + GetArchivedFileName(fileName));
                        }
                    }
                    File.AppendAllText((filePath), FormatText(message, messageLevel) + Environment.NewLine);
                }
                semaphore.Release();
            });
        }

        // Generates the next archived filename.
        private string GetArchivedFileName (string fileName)
        {
            fileName = fileName.Insert(fileName.IndexOf("."), $".{archiveNumber}");
            archiveNumber++;
            return fileName;
        }
    }
}
