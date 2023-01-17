using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LoggerClassLib
{
    public abstract class Logger
    {
        
        public MessageLevelOptions messageLevel;
        public MessageLevelOptions levelFilter { get; set; }

        public Logger(MessageLevelOptions levelFilter)
        {
            this.levelFilter = levelFilter;
        }

        // Abstract methods overriden in the subclasses.
        protected abstract void Log(string message, MessageLevelOptions messageLevel);
        protected abstract Task LogAsync(string message, MessageLevelOptions messageLevel);

        // Methods which should be called to log different levels of messages, sync and async
        public void Info(string message) { Log(message, MessageLevelOptions.Info); }
        public void Error(string message) { Log(message, MessageLevelOptions.Error); }
        public void Debug(string message) { Log(message, MessageLevelOptions.Debug); }
        public async Task InfoAsync(string message) { await LogAsync(message, MessageLevelOptions.Info); }
        public async Task ErrorAsync(string message) { await LogAsync(message, MessageLevelOptions.Error); }
        public async Task DebugAsync(string message) { await LogAsync(message, MessageLevelOptions.Debug); }

        // Transforms the raw message into the formatted string sent out to the logger, with datetime and message level added.
        protected static string FormatText(string inputText, MessageLevelOptions messageLevel)
        {
            DateTime now = DateTime.Now;
            inputText = $"{now.ToString()} [{messageLevel}] {inputText}";
            return inputText;
        }

        // Filters out the irrelevant messages base on the level filter setting of the logger
        protected bool CheckLevelFilter (MessageLevelOptions messageLevel)
        {
            if (messageLevel == MessageLevelOptions.Error ||
                levelFilter == MessageLevelOptions.Debug ||
                (messageLevel == MessageLevelOptions.Info && levelFilter == MessageLevelOptions.Info))
            { 
                return true;   
            }
            else return false;
        }
    }
}