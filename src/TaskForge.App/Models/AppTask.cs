/*
    App Task represents a named automation task consisting of a sequence
    of executable actions. In Laymen's terms, a recipe or script.
*/

namespace TaskForge.App.Models;

public class AppTask
{
    public string Name { get; set; } = string.Empty;

    public List<TaskAction> Actions { get; set; } = new();
}