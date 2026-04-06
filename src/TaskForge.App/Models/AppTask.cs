/*
    App Task represents a named automation task consisting of a sequence
    of executable actions. In Laymen's terms, a recipe or script.
*/

namespace TaskForge.App.Models;

/// <summary>
/// Represents a named automation task composed of multiple executable actions.
/// </summary>
public class AppTask
{
    /// <summary>
    /// Gets or sets the task name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the list of actions to execute.
    /// </summary>
    public List<TaskAction> Actions { get; set; } = new();
}