/*
    Logger Service is responsible for writing application log messages.
    It supports structured console logging with timestamps and log levels,
    and can be extended later to support file-based logging.
*/

using TaskForge.App.Models;
using TaskForge.App.Utils;

namespace TaskForge.App.Services;

public class LoggerService
{
    private readonly object _lock = new();

    /// <summary>
    /// Writes an informational log message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogInfo(string message)
    {
        Log(message, LogLevel.Info);
    }

    /// <summary>
    /// Writes a debug log message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogDebug(string message)
    {
        Log(message, LogLevel.Debug);
    }

    /// <summary>
    /// Writes a warning log message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogWarning(string message)
    {
        Log(message, LogLevel.Warning);
    }

    /// <summary>
    /// Writes an error log message.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public void LogError(string message)
    {
        Log(message, LogLevel.Error);
    }

    /// <summary>
    /// Writes a log message using the specified log level.
    /// </summary>
    /// <param name="message">The message to log.</param>
    /// <param name="level">The severity level.</param>
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
            Console.WriteLine(formattedMessage);
        }
    }
}