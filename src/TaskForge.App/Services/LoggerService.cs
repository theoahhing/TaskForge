/*
    Logger Service is responsible for writing application log messages.
    It supports structured console logging and file-based logging with
    timestamps and severity levels.
*/

using TaskForge.App.Models;
using TaskForge.App.Utils;

namespace TaskForge.App.Services;

public class LoggerService
{
    private readonly object _lock = new();

    private readonly string _logDirectory = "Logs";
    private readonly string _logFilePath;

    public LoggerService()
    {
        // Ensure log directory exists
        Directory.CreateDirectory(_logDirectory);

        // Create log file name based on date
        string fileName = $"TaskForge_{DateTime.Now:yyyy-MM-dd}.log";
        _logFilePath = Path.Combine(_logDirectory, fileName);
    }

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    public void LogInfo(string message)
    {
        Log(message, LogLevel.Info);
    }

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    public void LogDebug(string message)
    {
        Log(message, LogLevel.Debug);
    }

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    public void LogWarning(string message)
    {
        Log(message, LogLevel.Warning);
    }

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    public void LogError(string message)
    {
        Log(message, LogLevel.Error);
    }

    /// <summary>
    /// Writes a log message with a specified severity level.
    /// </summary>
    public void Log(string message, LogLevel level)
    {
        message = StringUtil.SafeTrim(message);

        if (StringUtil.IsNullOrWhiteSpace(message))
        {
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string formattedMessage = $"[{timestamp}] [{level}] {message}";

        lock (_lock)
        {
            // Console output
            Console.WriteLine(formattedMessage);

            // File output
            try
            {
                File.AppendAllText(_logFilePath, formattedMessage + Environment.NewLine);
            }
            catch
            {
                // Avoid crashing the app due to logging failure
                Console.WriteLine("Failed to write to log file.");
            }
        }
    }
}