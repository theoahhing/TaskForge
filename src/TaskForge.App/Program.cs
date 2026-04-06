/*
    Program is the entry point of the application. It is responsible for
    initializing services and executing test or runtime logic.
*/

using TaskForge.App.Services;

Console.WriteLine("=== TaskForge ===");

LoggerService loggerService = new();
ProcessService processService = new();
TaskRunnerService taskRunner = new(processService, loggerService);
TaskConfigService configService = new();

string taskFilePath = Path.Combine("Config", "task.json");

loggerService.LogInfo($"Current directory: {Directory.GetCurrentDirectory()}");
loggerService.LogInfo($"Looking for: {Path.GetFullPath(taskFilePath)}");

var task = configService.LoadTaskFromFile(taskFilePath);
var result = taskRunner.RunTask(task);

loggerService.LogInfo("=== Task Summary ===");
loggerService.LogInfo($"Task Name: {result.TaskName}");
loggerService.LogInfo($"Success: {result.Success}");
loggerService.LogInfo($"Total Actions: {result.TotalActions}");
loggerService.LogInfo($"Successful Actions: {result.SuccessfulActions}");
loggerService.LogInfo($"Failed Actions: {result.FailedActions}");

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();