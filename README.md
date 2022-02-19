# WallpaperSleep


## About

Wallpaper Engine seems to continue playing back your wallpaper, even when the
screen is locked or the screensaver is active. This may not be desirable in some
cases.

This program attempts to mitigate this, by launching a child process and using
the application rules in Wallpaper Engine.


## What does it exactly do?

WallpaperSleep checks whether:

- Wallpaper Engine's screensaver is running every 3 seconds
- Windows is locked

If either are true, a child process (`WallpaperSleep.ChildProcess.exe`) will be
launched, which can be used to trigger an application rule in Wallpaper Engine's
settings.

While Wallpaper Engine abides by the application rules, the screensaver ignores
them. Thus, only the desktop wallpaper pauses.


### Caveats

- Both WallpaperSleep & Wallpaper Engine take a short while to pause & resume
  animations, as both applications only checks running processes every few
  seconds.


## Setup


### Installation

This program is only compatible with Windows.

1. Install the [.NET 6 Runtime or SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Download the [latest release](https://github.com/MichailiK/WallpaperSleep/releases/latest)
3. Extract both files
4. Run `WallpaperSleep.exe`

Note that WallpaperSleep will no longer run when logging out or shutting down.
View the Persistence section below to run WallpaperSleep upon startup.


### Wallpaper Engine

1. Open Wallpaper Engine's settings by clicking on its tray icon and selecting
   "Settings"
2. Click on the "Edit" button right next to the application rules text
3. Click on the "Create new rule" button
4. Enter `WallpaperSleep.ChildProcess.exe` for the application name
5. Keep the condition set to "Is running"
6. Choose any any wallpaper playback settings you desire
   > If you chose "Pause" you may notice how the Wallpaper Playback may be
   > labeled "Paused per monitor". This is false: Wallpaper Engine pauses
   > playback on all monitors, likely because WallpaperSleep doesn't create any
   > windows.

From now on, Wallpaper Engine will pause playback whenever the screensaver is
active or Windows is locked, whenever WallpaperSleep is running.


### Persistence

WallpaperSleep does not offer functionality to add itself to the startup. It can
be done manually, though:

1. Right click `WallpaperSleep.exe` -> Send to -> Desktop (create shortcut)
2. Navigate to `%appdata%\Roaming\Microsoft\Windows\Start Menu\Programs\Startup`
3. Move the shortcut, created in step 1, into the Startup folder

Note that if you move the files to a different location, you will need to redo
the shortcut.


## Building

Requirements:
- Git
- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

1. Clone this repository
   ```shell
   $ git clone https://github.com/MichailiK/WallpaperSleep.git
   ```
2. `cd` to the directory & use `dotnet publish` to create a release build:
   ```shell
   $ cd WallpaperSleep
   $ dotnet publish WallpaperSleep -c Release --output Publish
   ```
3. If everything has gone well, you should find the executables in the `Publish`
   directory.
