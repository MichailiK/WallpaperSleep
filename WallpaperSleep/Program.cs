// Copyright (c) Michaili K. This file is licensed under the MIT license.
// The full license text can be found at the root of this repository.

using System.Diagnostics;
using WallpaperSleep;


const string childProcessPath = "WallpaperSleep.ChildProcess.exe";
const string screensaverProcessName = "wallpaper32.exe";
const string screensaverProcessorArgs = "-screensaver";

if (!File.Exists(childProcessPath))
    return 1;

var processWatcher = new ProcessWatcher(screensaverProcessName, screensaverProcessorArgs);
var sessionWatcher = new SessionWatcher();

Process? childProcess = null;
var processLock = new object();

processWatcher.ProcessStarted += WatcherOnEvent;
processWatcher.ProcessStopped += WatcherOnEvent;
processWatcher.Start();

sessionWatcher.Lock += WatcherOnEvent;
sessionWatcher.Unlock += WatcherOnEvent;
sessionWatcher.Start();


Thread.Sleep(Timeout.Infinite);

return 0;


void WatcherOnEvent(object? sender, EventArgs e)
{
    UpdateChildProcess();
}


void UpdateChildProcess(bool shouldExit = false)
{
    lock (processLock)
    {
        if (shouldExit || sessionWatcher.IsLocked || processWatcher.IsRunning)
        {
            if (childProcess is null || childProcess.HasExited)
                childProcess = Process.Start(childProcessPath, Environment.ProcessId.ToString());
        }
        else
        {
            if (childProcess is not null && !childProcess.HasExited)
                childProcess.Kill();

            childProcess = null;
        }
    }
}
