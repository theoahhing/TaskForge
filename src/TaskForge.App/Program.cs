/*
    Program is the entry point of the application. It is responsible for
    initializing services and executing test or runtime logic.
*/

using TaskForge.App.Services;

Console.WriteLine("=== TaskForge ===");

// Build services
ProcessService processService = new();
TaskRunnerService taskRunner = new(processService);
TaskConfigService configService = new();

// Build file path
string taskFilePath = Path.Combine("Config", "task.json");

// Debug (optional but useful)
Console.WriteLine($"Current directory: {Directory.GetCurrentDirectory()}");
Console.WriteLine($"Looking for: {Path.GetFullPath(taskFilePath)}");

// Load task from JSON
var task = configService.LoadTaskFromFile(taskFilePath);

// Run task
var result = taskRunner.RunTask(task);

// Print summary
Console.WriteLine("\n=== Task Summary ===");
Console.WriteLine($"Task Name: {result.TaskName}");
Console.WriteLine($"Success: {result.Success}");
Console.WriteLine($"Total Actions: {result.TotalActions}");
Console.WriteLine($"Successful Actions: {result.SuccessfulActions}");
Console.WriteLine($"Failed Actions: {result.FailedActions}");

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();