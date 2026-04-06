/*
    Task Action class defines a single executable step within a task, including
    its type and required parameters.
*/

namespace TaskForge.App.Models;

public class TaskAction
{
    public string Type { get; set; } = string.Empty;

    public string? Value { get; set; }

    public int DelayMs { get; set; }
}