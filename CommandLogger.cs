using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CommandLogger
{
    private readonly string _logFilePath;
    private readonly object _lock = new object();

    public CommandLogger()
    {
        // Set log file path to Desktop/OLVimages/OLVCommandlog.txt
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string folderPath = Path.Combine(desktopPath, "OLVimages");
        Directory.CreateDirectory(folderPath); // Create folder if it doesn't exist
        _logFilePath = Path.Combine(folderPath, "OLVCommandLog.txt");
    }

    // Log a command with only timestamp and command string
    public void LogCommand(string command)
    {
        lock (_lock) // Ensure thread-safety
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string logEntry = $"{command}\n";

            try
            {
                File.AppendAllText(_logFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }
    }

    // Read the log file for scripting purposes
    public List<string> ReadLog()
    {
        lock (_lock)
        {
            try
            {
                if (File.Exists(_logFilePath))
                {
                    return File.ReadAllLines(_logFilePath).ToList();
                }
                return new List<string>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading log: {ex.Message}");
                return new List<string>();
            }
        }
    }
}