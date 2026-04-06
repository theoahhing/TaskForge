/*
    Task Action represents a single step within an AppTask. It defines
    the type of action to execute (e.g. start, stop, wait) along with
    any required parameters.
*/

namespace TaskForge.App.Models;

/// <summary>
/// Represents a single executable action within an automation task.
/// </summary>
public class TaskAction
{
    /// <summary>
    /// Gets or sets the action type.
    /// </summary>
    public TaskActionType Type { get; set; }

    /// <summary>
    /// Gets or sets the action value, such as an executable path or process name.
    /// </summary>
    public string? Value { get; set; }

    /// <summary>
    /// Gets or sets the delay in milliseconds for wait actions.
    /// </summary>
    public int DelayMs { get; set; }
}