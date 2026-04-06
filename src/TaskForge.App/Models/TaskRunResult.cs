/*
    TaskRunResult stores the outcome of a task execution. It tracks
    whether the task succeeded, how many actions were processed,
    and any messages generated during execution.
*/

namespace TaskForge.App.Models;

public class TaskRunResult
{
    public string TaskName { get; set; } = string.Empty;

    public bool Success { get; set; }

    public int TotalActions { get; set; }

    public int SuccessfulActions { get; set; }

    public int FailedActions { get; set; }

    public List<string> Messages { get; set; } = new();
}