// Copyright (c) Michaili K. This file is licensed under the MIT license.
// The full license text can be found at the root of this repository.

using Microsoft.Win32;

namespace WallpaperSleep;

public class SessionWatcher : IDisposable
{
    public bool IsLocked { get; private set; }


    public void Dispose()
    {
        SystemEvents.SessionSwitch -= SystemEventsOnSessionSwitch;
        GC.SuppressFinalize(this);
    }

    public event EventHandler? Lock;
    public event EventHandler? Unlock;

    public void Start()
    {
        SystemEvents.SessionSwitch += SystemEventsOnSessionSwitch;
    }

    private void SystemEventsOnSessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
        switch (e.Reason)
        {
            case SessionSwitchReason.SessionLock:
                IsLocked = true;
                Lock?.Invoke(this, EventArgs.Empty);
                break;
            case SessionSwitchReason.SessionUnlock:
                IsLocked = false;
                Unlock?.Invoke(this, EventArgs.Empty);
                break;
        }
    }
}
