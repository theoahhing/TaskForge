/*
    Exe Scanner Service class will...
*/

using TaskForge.App.Utils;

namespace TaskForge.App.Services;

public class ExeScannerService
{
    /// <summary>
    /// Finds all executable files in the specified directory.
    /// </summary>
    /// <param name="directoryPath">The directory to scan.</param>
    /// <returns>A list of full file paths for all .exe files found.</returns>
    /// <exception cref="ArgumentException">Thrown when the directory path is null or blank.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory does not exist.</exception>
    public List<string> FindExecutables(string directoryPath)
    {
        directoryPath = StringUtil.SafeTrim(directoryPath);

        if (StringUtil.IsNullOrWhiteSpace(directoryPath))
        {
            throw new ArgumentException("Directory path cannot be null or blank.", nameof(directoryPath));
        }

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
        }

        return Directory
            .GetFiles(directoryPath, "*.exe", SearchOption.TopDirectoryOnly)
            .OrderBy(Path.GetFileName)
            .ToList();
    }

    /// <summary>
    /// Finds all executable files in the specified directory and its subdirectories.
    /// </summary>
    /// <param name="directoryPath">The root directory to scan.</param>
    /// <returns>A list of full file paths for all .exe files found.</returns>
    /// <exception cref="ArgumentException">Thrown when the directory path is null or blank.</exception>
    /// <exception cref="DirectoryNotFoundException">Thrown when the directory does not exist.</exception>
    public List<string> FindExecutablesRecursive(string directoryPath)
    {
        directoryPath = StringUtil.SafeTrim(directoryPath);

        if (StringUtil.IsNullOrWhiteSpace(directoryPath))
        {
            throw new ArgumentException("Directory path cannot be null or blank.", nameof(directoryPath));
        }

        if (!Directory.Exists(directoryPath))
        {
            throw new DirectoryNotFoundException($"Directory not found: {directoryPath}");
        }

        return Directory
            .GetFiles(directoryPath, "*.exe", SearchOption.AllDirectories)
            .OrderBy(Path.GetFileName)
            .ToList();
    }

    /// <summary>
    /// Checks whether the given file path points to an executable file.
    /// </summary>
    /// <param name="filePath">The file path to validate.</param>
    /// <returns>True if the file exists and has a .exe extension; otherwise false.</returns>
    public bool IsExecutable(string filePath)
    {
        filePath = StringUtil.SafeTrim(filePath);

        if (StringUtil.IsNullOrWhiteSpace(filePath))
        {
            return false;
        }

        if (!File.Exists(filePath))
        {
            return false;
        }

        return StringUtil.EqualsIgnoreCase(Path.GetExtension(filePath), ".exe");
    }

    /// <summary>
    /// Gets just the executable file names from a list of full file paths.
    /// </summary>
    /// <param name="executablePaths">The list of executable file paths.</param>
    /// <returns>A list of executable file names.</returns>
    public List<string> GetExecutableNames(IEnumerable<string> executablePaths)
    {
        if (executablePaths == null)
        {
            throw new ArgumentNullException(nameof(executablePaths));
        }

        return executablePaths
            .Where(path => !StringUtil.IsNullOrWhiteSpace(path))
            .Select(Path.GetFileName)
            .Where(name => !StringUtil.IsNullOrWhiteSpace(name))
            .Cast<string>()
            .OrderBy(name => name)
            .ToList();
    }
}