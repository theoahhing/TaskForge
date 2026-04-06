/*
    Task Runner Service is responsible for executing automation tasks.
    It processes each action in sequence and coordinates the use of
    underlying services such as ProcessService.
*/

using TaskForge.App.Models;
using TaskForge.App.Utils;

namespace TaskForge.App.Services;

/// <summary>
/// Executes automation tasks by processing task actions in sequence.
/// </summary>
public class TaskRunnerService
{
    private readonly ProcessService _processService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TaskRunnerService"/> class.
    /// </summary>
    /// <param name="processService">The process service used for process-related actions.</param>
    /// <exception cref="ArgumentNullException">Thrown when the process service is null.</exception>
    public TaskRunnerService(ProcessService processService)
    {
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
    }

    /// <summary>
    /// Executes the supplied task and returns a detailed result.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <returns>A detailed result describing the outcome of the task run.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the task is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the task is invalid.</exception>
    public TaskRunResult RunTask(AppTask task)
    {
        ValidateTask(task);

        TaskRunResult result = new()
        {
            TaskName = task.Name,
            TotalActions = task.Actions.Count
        };

        AddMessage(result, $"Starting task: {task.Name}");

        for (int i = 0; i < task.Actions.Count; i++)
        {
            TaskAction action = task.Actions[i];

            try
            {
                ValidateAction(action, i);

                bool actionSucceeded = ExecuteAction(action, result);

                if (actionSucceeded)
                {
                    result.SuccessfulActions++;
                    AddMessage(result, $"Action {i + 1} succeeded: {GetActionDescription(action)}");
                }
                else
                {
                    result.FailedActions++;
                    AddMessage(result, $"Action {i + 1} failed: {GetActionDescription(action)}");
                }
            }
            catch (Exception ex)
            {
                result.FailedActions++;
                AddMessage(result, $"Action {i + 1} threw an error: {ex.Message}");
            }
        }

        result.Success = result.FailedActions == 0;

        AddMessage(
            result,
            result.Success
                ? $"Task completed successfully: {task.Name}"
                : $"Task completed with errors: {task.Name}");

        return result;
    }

    /// <summary>
    /// Executes a single task action.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="result">The task result being updated.</param>
    /// <returns>True if the action completed successfully; otherwise false.</returns>
    private bool ExecuteAction(TaskAction action, TaskRunResult result)
    {
        switch (action.Type)
        {
            case TaskActionType.Start:
                return ExecuteStartAction(action, result);

            case TaskActionType.Stop:
                return ExecuteStopAction(action, result);

            case TaskActionType.Wait:
                return ExecuteWaitAction(action, result);

            default:
                AddMessage(result, $"Unsupported action type: {action.Type}");
                return false;
        }
    }

    /// <summary>
    /// Executes a start action by launching an executable.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="result">The task result being updated.</param>
    /// <returns>True if the process started successfully; otherwise false.</returns>
    private bool ExecuteStartAction(TaskAction action, TaskRunResult result)
    {
        string executablePath = StringUtil.SafeTrim(action.Value);

        if (StringUtil.IsNullOrWhiteSpace(executablePath))
        {
            AddMessage(result, "Start action requires an executable path.");
            return false;
        }

        var process = _processService.StartProcess(executablePath);

        if (process == null)
        {
            AddMessage(result, $"Failed to start executable: {executablePath}");
            return false;
        }

        AddMessage(result, $"Started process '{process.ProcessName}' (PID: {process.Id}).");
        return true;
    }

    /// <summary>
    /// Executes a stop action by terminating processes with the specified name.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="result">The task result being updated.</param>
    /// <returns>True if at least one process was stopped; otherwise false.</returns>
    private bool ExecuteStopAction(TaskAction action, TaskRunResult result)
    {
        string processName = StringUtil.SafeTrim(action.Value);

        if (StringUtil.IsNullOrWhiteSpace(processName))
        {
            AddMessage(result, "Stop action requires a process name.");
            return false;
        }

        int stoppedCount = _processService.StopProcessByName(processName);

        if (stoppedCount <= 0)
        {
            AddMessage(result, $"No processes were stopped for name '{processName}'.");
            return false;
        }

        AddMessage(result, $"Stopped {stoppedCount} process(es) named '{processName}'.");
        return true;
    }

    /// <summary>
    /// Executes a wait action by pausing the current thread.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <param name="result">The task result being updated.</param>
    /// <returns>True if the wait completed successfully; otherwise false.</returns>
    private bool ExecuteWaitAction(TaskAction action, TaskRunResult result)
    {
        if (action.DelayMs < 0)
        {
            AddMessage(result, "Wait action cannot have a negative delay.");
            return false;
        }

        AddMessage(result, $"Waiting for {action.DelayMs} ms.");
        Thread.Sleep(action.DelayMs);
        return true;
    }

    /// <summary>
    /// Validates the task before execution.
    /// </summary>
    /// <param name="task">The task to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown when the task is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the task is invalid.</exception>
    private void ValidateTask(AppTask task)
    {
        if (task == null)
        {
            throw new ArgumentNullException(nameof(task));
        }

        if (StringUtil.IsNullOrWhiteSpace(task.Name))
        {
            throw new ArgumentException("Task name cannot be null or blank.", nameof(task));
        }

        if (task.Actions == null || task.Actions.Count == 0)
        {
            throw new ArgumentException("Task must contain at least one action.", nameof(task));
        }
    }

    /// <summary>
    /// Validates an individual action before execution.
    /// </summary>
    /// <param name="action">The action to validate.</param>
    /// <param name="index">The action index.</param>
    /// <exception cref="ArgumentNullException">Thrown when the action is null.</exception>
    private void ValidateAction(TaskAction action, int index)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), $"Action at index {index} is null.");
        }
    }

    /// <summary>
    /// Builds a user-friendly description for a task action.
    /// </summary>
    /// <param name="action">The action to describe.</param>
    /// <returns>A short action description.</returns>
    private string GetActionDescription(TaskAction action)
    {
        return action.Type switch
        {
            TaskActionType.Start => $"Start '{action.Value}'",
            TaskActionType.Stop => $"Stop '{action.Value}'",
            TaskActionType.Wait => $"Wait {action.DelayMs} ms",
            _ => $"Unknown action '{action.Type}'"
        };
    }

    /// <summary>
    /// Adds a message to the result and writes it to the console.
    /// </summary>
    /// <param name="result">The task result.</param>
    /// <param name="message">The message to add.</param>
    private void AddMessage(TaskRunResult result, string message)
    {
        result.Messages.Add(message);
        Console.WriteLine(message);
    }
}