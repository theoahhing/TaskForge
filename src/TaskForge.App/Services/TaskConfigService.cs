/*
    TaskConfigService is responsible for loading and saving task definitions
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
    /// <summary>
    /// Loads a task from a JSON file.
    /// </summary>
    /// <param name="filePath">The path to the JSON file.</param>
    /// <returns>The loaded AppTask.</returns>
    /// <exception cref="ArgumentException">Thrown when the file path is invalid.</exception>
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
            throw new FileNotFoundException($"Task file not found: {filePath}");
        }

        string json = File.ReadAllText(filePath);

        AppTask? task = JsonSerializer.Deserialize<AppTask>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() }
        });

        if (task == null)
        {
            throw new InvalidOperationException("Failed to deserialize task from JSON.");
        }

        return task;
    }

    /// <summary>
    /// Saves a task to a JSON file.
    /// </summary>
    /// <param name="task">The task to save.</param>
    /// <param name="filePath">The file path to save to.</param>
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

        string json = JsonSerializer.Serialize(task, new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
        });

        File.WriteAllText(filePath, json);
    }
}