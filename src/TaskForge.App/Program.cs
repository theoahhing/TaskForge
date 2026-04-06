/*
    Program file is the entry point of the application responsible for initializing
    services and executing test or runtime logic.
*/

using System.Diagnostics;
using TaskForge.App.Models;
using TaskForge.App.Services;

using TaskForge.App.Models;
using TaskForge.App.Services;

ProcessService processService = new();
TaskRunnerService taskRunnerService = new(processService);

AppTask task = new()
{
    Name = "Notepad Test Task",
    Actions = new List<TaskAction>
    {
        new TaskAction
        {
            Type = "start",
            Value = @"C:\Windows\System32\notepad.exe"
        },
        new TaskAction
        {
            Type = "wait",
            DelayMs = 5000
        },
        new TaskAction
        {
            Type = "stop",
            Value = "notepad"
        }
    }
};

taskRunnerService.RunTask(task);

Console.WriteLine("Press any key to exit...");
Console.ReadKey();