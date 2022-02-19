// Copyright (c) Michaili K. This file is licensed under the MIT license.
// The full license text can be found at the root of this repository.

using System.Diagnostics.CodeAnalysis;
using ORMi;

namespace WallpaperSleep;

public class ProcessWatcher : IDisposable
{
    private const string WmiScope = "root\\CIMV2";

    private readonly string _processName;
    private readonly string? _processArguments;

    private readonly Thread _thread;
    private readonly WMIHelper _wmiHelper = new(WmiScope);

    private bool _disposed;

    public bool IsRunning { get; private set; }

    public event EventHandler? ProcessStarted;
    public event EventHandler? ProcessStopped;


    public ProcessWatcher(string processName, string? processorArguments)
    {
        _processName = processName ?? throw new ArgumentNullException(nameof(processName));
        _processArguments = processorArguments;
        _thread = new Thread(ThreadCallback);
    }

    public void Start()
    {
        _thread.Start();
    }

    private void ThreadCallback()
    {
        while (!_disposed)
        {
            var matches = FindMatches().ToList();

            if (matches.Any() && !IsRunning)
            {
                IsRunning = true;
                ProcessStarted?.Invoke(this, EventArgs.Empty);
            }
            else if (!matches.Any() && IsRunning)
            {
                IsRunning = false;
                ProcessStopped?.Invoke(this, EventArgs.Empty);
            }

            Thread.Sleep(3000);
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        _thread.Interrupt();
        GC.SuppressFinalize(this);
    }


    // Fetches every running process and return processes that match the provided process name and arguments.
    private IEnumerable<WmiProcess> FindMatches()
    {
        var query = _wmiHelper.Query<WmiProcess>($"SELECT * FROM Win32_Process WHERE Name = '{_processName}'");
        var matches = query.Where(IsMatch).ToList();
        return matches;
    }

    // Determines whether the nullable WmiProcess matches with the provided process name and arguments.
    private bool IsMatch([NotNullWhen(true)] WmiProcess? wmiProcess)
    {
        if (wmiProcess is null) return false;
        if (wmiProcess.Name != _processName) return false;
        return _processArguments is null || wmiProcess.CommandLine.Contains(_processArguments);
    }


#nullable disable

    [WMIClass("Win32_Process")]
    public class WmiProcess
    {
        public string Name { get; set; }
        public string CommandLine { get; set; }
    }

#nullable enable
}
