/*
    Program file used to... 
*/

using TaskForge.App.Services;

Console.WriteLine("=== TaskForge ===");

ExeScannerService exeScannerService = new();

Console.WriteLine("Enter directory path to scan: ");
string? inputPath = Console.ReadLine();

try
{
    List<string> executables = exeScannerService.FindExecutables(inputPath ?? "");

    Console.WriteLine($"\nFound {executables.Count} executable(s):\n");

    foreach( string executable in executables)
    {
        Console.WriteLine(executable);
    }
}
catch(Exception ex)
{
    Console.WriteLine($"\n[ERROR]: {ex.Message}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();