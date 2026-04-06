/*
    Program file used to... 
*/

using System.Diagnostics;
using TaskForge.App.Services;

ProcessService processService = new();

var processes = processService.GetRunningProcesses();

Console.WriteLine("=== Running Processes ===\n");

// Show first 50 for sanity
var displayList = processes.Take(200).ToList();

for (int i = 0; i < displayList.Count; i++)
{
    var p = displayList[i];
    Console.WriteLine($"{i}: {p.ProcessName} (PID: {p.Id})");
}

Console.Write("\nSelect process index to kill: ");
string? input = Console.ReadLine();

if (int.TryParse(input, out int index) && index >= 0 && index < displayList.Count)
{
    var selected = displayList[index];

    Console.WriteLine($"\nKilling: {selected.ProcessName} (PID: {selected.Id})");

    bool success = processService.StopProcessById(selected.Id);

    Console.WriteLine(success ? "Process stopped successfully." : "Failed to stop process.");
}
else
{
    Console.WriteLine("Invalid selection.");
}