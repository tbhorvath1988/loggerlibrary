# loggerlibrary
// Tested with a console application using this library:

using LoggerClassLib;

// Create logger object, like:
Logger log1 = LoggerFactory.CreateLoggerOfType(LoggerTypeOptions.Console, MessageLevelOptions.Debug);

// LoggerTypeOptions: Console, File, Stream
// MessageLevelOptions (filter, only messages including and above this level will be displayed): Error, Info, Debug

// After instantiation, logging methods can be called with:
// log1.Debug("message");
// log1.Info("message");
// log1.Error("message");

// ...or asynchronously with:
// Task t0 = log1.DebugAsync("message");
// Task t1 = log1.InfoAsync("message");
// Task t2 = log1.ErrorAsync("message");
// await t0; await t1; await t2 ...etc

// LogFilter can be set with:
// log1.levelFilter = MessageLevelOptions.{one of the options};
