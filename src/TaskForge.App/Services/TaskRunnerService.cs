/*
    Task Runner Service class executes automation tasks by processing and
    coordinating a sequence of actions using underlying services.
*/

using TaskForge.App.Models;
using TaskForge.App.Utils;

namespace TaskForge.App.Services;

public class TaskRunnerService
{
    private readonly ProcessService _processService;

    /// <summary>
    /// Creates a new task runner service.
    /// </summary>
    /// <param name="processService">The process service used to execute process-related actions.</param>
    /// <exception cref="ArgumentNullException">Thrown when the process service is null.</exception>
    public TaskRunnerService(ProcessService processService)
    {
        _processService = processService ?? throw new ArgumentNullException(nameof(processService));
    }

    /// <summary>
    /// Executes all actions in the provided task in sequence.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <returns>True if all actions completed successfully; otherwise false.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the task is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the task name is invalid or the task has no actions.</exception>
    public bool RunTask(AppTask task)
    {
        ValidateTask(task);

        Console.WriteLine($"Starting task: {task.Name}");

        bool allActionsSucceeded = true;

        for (int i = 0; i < task.Actions.Count; i++)
        {
            TaskAction action = task.Actions[i];

            try
            {
                ValidateAction(action, i);

                bool actionSucceeded = ExecuteAction(action);

                if (!actionSucceeded)
                {
                    allActionsSucceeded = false;
                    Console.WriteLine($"Action {i + 1} failed: {GetActionDescription(action)}");
                }
                else
                {
                    Console.WriteLine($"Action {i + 1} completed: {GetActionDescription(action)}");
                }
            }
            catch (Exception ex)
            {
                allActionsSucceeded = false;
                Console.WriteLine($"Action {i + 1} threw an error: {ex.Message}");
            }
        }

        Console.WriteLine(allActionsSucceeded
            ? $"Task completed successfully: {task.Name}"
            : $"Task completed with errors: {task.Name}");

        return allActionsSucceeded;
    }

    /// <summary>
    /// Executes a single task action.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>True if the action completed successfully; otherwise false.</returns>
    private bool ExecuteAction(TaskAction action)
    {
        string actionType = StringUtil.Normalize(action.Type);

        switch (actionType)
        {
            case "start":
                return ExecuteStartAction(action);

            case "stop":
                return ExecuteStopAction(action);

            case "wait":
                return ExecuteWaitAction(action);

            default:
                Console.WriteLine($"Unsupported action type: {action.Type}");
                return false;
        }
    }

    /// <summary>
    /// Executes a start action by launching an executable.
    /// </summary>
    /// <param name="action">The task action containing the executable path.</param>
    /// <returns>True if the process started successfully; otherwise false.</returns>
    private bool ExecuteStartAction(TaskAction action)
    {
        string executablePath = StringUtil.SafeTrim(action.Value);

        if (StringUtil.IsNullOrWhiteSpace(executablePath))
        {
            Console.WriteLine("Start action requires an executable path.");
            return false;
        }

        var process = _processService.StartProcess(executablePath);

        return process != null;
    }

    /// <summary>
    /// Executes a stop action by terminating processes with the specified name.
    /// </summary>
    /// <param name="action">The task action containing the process name.</param>
    /// <returns>True if at least one process was stopped; otherwise false.</returns>
    private bool ExecuteStopAction(TaskAction action)
    {
        string processName = StringUtil.SafeTrim(action.Value);

        if (StringUtil.IsNullOrWhiteSpace(processName))
        {
            Console.WriteLine("Stop action requires a process name.");
            return false;
        }

        int stoppedCount = _processService.StopProcessByName(processName);

        return stoppedCount > 0;
    }

    /// <summary>
    /// Executes a wait action by pausing the current thread.
    /// </summary>
    /// <param name="action">The task action containing the wait duration in milliseconds.</param>
    /// <returns>True if the wait completed successfully; otherwise false.</returns>
    private bool ExecuteWaitAction(TaskAction action)
    {
        if (action.DelayMs < 0)
        {
            Console.WriteLine("Wait action cannot have a negative delay.");
            return false;
        }

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
    /// Validates a task action before execution.
    /// </summary>
    /// <param name="action">The action to validate.</param>
    /// <param name="index">The action index in the task.</param>
    /// <exception cref="ArgumentNullException">Thrown when the action is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the action type is invalid.</exception>
    private void ValidateAction(TaskAction action, int index)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action), $"Action at index {index} is null.");
        }

        if (StringUtil.IsNullOrWhiteSpace(action.Type))
        {
            throw new ArgumentException($"Action at index {index} has no type.");
        }
    }

    /// <summary>
    /// Builds a user-friendly description of a task action.
    /// </summary>
    /// <param name="action">The action to describe.</param>
    /// <returns>A short action description.</returns>
    private string GetActionDescription(TaskAction action)
    {
        string actionType = StringUtil.Normalize(action.Type);

        return actionType switch
        {
            "start" => $"Start '{action.Value}'",
            "stop" => $"Stop '{action.Value}'",
            "wait" => $"Wait {action.DelayMs} ms",
            _ => $"Unknown action '{action.Type}'"
        };
    }
}