/*
    Task Config Service is responsible for loading and saving task definitions
    from external sources such as JSON files. It allows tasks to be configured
    without modifying application code.
*/

using System.Text.Json;
using System.Text.Json.Serialization;
using TaskForge.App.Models;
using TaskForge.App.Utils;

namespace TaskForge.App.Services;

public class TaskConfigService
{
    private readonly LoggerService _loggerService;

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() }
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskConfigService"/> class.
    /// </summary>
    /// <param name="loggerService">The logger service used for configuration logging.</param>
    /// <exception cref="ArgumentNullException">Thrown when the logger service is null.</exception>
    public TaskConfigService(LoggerService loggerService)
    {
        _loggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
    }

    /// <summary>
    /// Loads a task definition from a JSON file.
    /// </summary>
    /// <param name="filePath">The path to the task JSON file.</param>
    /// <returns>The deserialized task definition.</returns>
    /// <exception cref="ArgumentException">Thrown when the file path is null or blank.</exception>
    /// <exception cref="FileNotFoundException">Thrown when the file does not exist.</exception>
    /// <exception cref="InvalidOperationException">Thrown when deserialization fails.</exception>
    public AppTask LoadTaskFromFile(string filePath)
    {
        filePath = StringUtil.SafeTrim(filePath);

        if (StringUtil.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or blank.", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            _loggerService.LogError($"Task file not found: {filePath}");
            throw new FileNotFoundException($"Task file not found: {filePath}");
        }

        _loggerService.LogInfo($"Loading task from file: {filePath}");

        string json = File.ReadAllText(filePath);

        AppTask? task = JsonSerializer.Deserialize<AppTask>(json, _jsonSerializerOptions);

        if (task == null)
        {
            _loggerService.LogError($"Failed to deserialize task from file: {filePath}");
            throw new InvalidOperationException("Failed to deserialize task from JSON.");
        }

        _loggerService.LogInfo($"Successfully loaded task '{task.Name}' from file: {filePath}");

        return task;
    }

    /// <summary>
    /// Saves a task definition to a JSON file.
    /// </summary>
    /// <param name="task">The task to save.</param>
    /// <param name="filePath">The destination file path.</param>
    /// <exception cref="ArgumentNullException">Thrown when the task is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the file path is null or blank.</exception>
    public void SaveTaskToFile(AppTask task, string filePath)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        filePath = StringUtil.SafeTrim(filePath);

        if (StringUtil.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or blank.", nameof(filePath));
        }

        _loggerService.LogInfo($"Saving task '{task.Name}' to file: {filePath}");

        string? directoryPath = Path.GetDirectoryName(filePath);

        if (!StringUtil.IsNullOrWhiteSpace(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string json = JsonSerializer.Serialize(task, _jsonSerializerOptions);

        File.WriteAllText(filePath, json);

        _loggerService.LogInfo($"Successfully saved task '{task.Name}' to file: {filePath}");
    }
}