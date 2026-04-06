/*
    Process Service class handles system process operations including starting,
    stopping, and querying runing processes.
*/

using System.Diagnostics;
using TaskForge.App.Utils;

namespace TaskForge.App.Services;

public class ProcessService
{

    /// <summary>
    /// Retrieves all running processes.
    /// </summary>
    /// <returns>A list of running processes.</returns>
    public List<Process> GetRunningProcesses()
    {
        return Process
            .GetProcesses()
            .OrderBy(p => p.ProcessName)
            .ToList();
    }

    /// <summary>
    /// Stops a process by its process ID.
    /// </summary>
    /// <param name="processId">The process ID.</param>
    /// <returns>True if the process was stopped successfully; otherwise false.</returns>
    public bool StopProcessById(int processId)
    {
        try
        {
            Process process = Process.GetProcessById(processId);

            process.Kill();
            process.WaitForExit();

            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed to stop process with ID {processId}: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Starts a process using the specified executable path.
    /// </summary>
    /// <param name="exePath">The full path to the executable.</param>
    /// <param name="arguments">Optional arguments to pass to the executable.</param>
    /// <returns>The started process, or null if the process could not be started.</returns>
    /// <exception cref="ArgumentException">Thrown when the path is null or invalid.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the executable does not exist.</exception>
    public Process? StartProcess(string exePath, string? arguments = null)
    {
        exePath = StringUtil.SafeTrim(exePath);

        if (StringUtil.IsNullOrWhiteSpace(exePath))
        {
            throw new ArgumentException("Executable path cannot be null or blank.", nameof(exePath));
        }

        if (!File.Exists(exePath))
        {
            throw new FileNotFoundException($"Executable not found: {exePath}");
        }

        try
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = exePath,
                Arguments = arguments ?? string.Empty,
                UseShellExecute = true
            };

            return Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start process: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Stops all processes with the specified process name.
    /// </summary>
    /// <param name="processName">The name of the process (without .exe).</param>
    /// <returns>The number of processes successfully terminated.</returns>
    /// <exception cref="ArgumentException">Thrown when the process name is null or blank.</exception>
    public int StopProcessByName(string processName)
    {
        processName = StringUtil.SafeTrim(processName);

        if (StringUtil.IsNullOrWhiteSpace(processName))
        {
            throw new ArgumentException("Process name cannot be null or blank.", nameof(processName));
        }

        int killedCount = 0;

        Process[] processes = Process.GetProcessesByName(processName);

        foreach (Process process in processes)
        {
            try
            {
                process.Kill();
                process.WaitForExit();
                killedCount++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to stop process '{process.ProcessName}': {ex.Message}");
            }
        }

        return killedCount;
    }

    /// <summary>
    /// Checks whether a process with the specified name is currently running.
    /// </summary>
    /// <param name="processName">The name of the process (without .exe).</param>
    /// <returns>True if at least one process is running; otherwise false.</returns>
    public bool IsProcessRunning(string processName)
    {
        processName = StringUtil.SafeTrim(processName);

        if (StringUtil.IsNullOrWhiteSpace(processName))
        {
            return false;
        }

        return Process.GetProcessesByName(processName).Length > 0;
    }

    /// <summary>
    /// Retrieves a list of all currently running process names.
    /// </summary>
    /// <returns>A list of process names.</returns>
    public List<string> GetRunningProcessNames()
    {
        return Process
            .GetProcesses()
            .Select(p => p.ProcessName)
            .Distinct()
            .OrderBy(name => name)
            .ToList();
    }

    /// <summary>
    /// Attempts to extract the process name from an executable path.
    /// </summary>
    /// <param name="exePath">The full path to the executable.</param>
    /// <returns>The process name without extension.</returns>
    public string GetProcessNameFromPath(string exePath)
    {
        exePath = StringUtil.SafeTrim(exePath);

        if (StringUtil.IsNullOrWhiteSpace(exePath))
        {
            return string.Empty;
        }

        return Path.GetFileNameWithoutExtension(exePath);
    }
}