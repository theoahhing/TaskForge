/*
    TaskActionType defines the supported types of actions that can be
    executed within a task. This ensures type safety and prevents errors
    caused by using raw strings.
*/

namespace TaskForge.App.Models;

public enum TaskActionType
{
    Start,
    Stop,
    Wait
}