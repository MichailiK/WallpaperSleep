// Copyright (c) Michaili K. This file is licensed under the MIT license.
// The full license text can be found at the root of this repository.

using System.Diagnostics;

if (args.Length == 0 || !int.TryParse(args[0], out var pid))
    return 1;

Process parentProcess;
try
{
    parentProcess = Process.GetProcessById(pid);
}
catch (ArgumentException)
{
    return 1;
}

parentProcess.WaitForExit();

return 0;
