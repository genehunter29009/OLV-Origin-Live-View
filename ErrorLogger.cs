using System;
using System.IO;

public class ErrorLogger
{
    private readonly string logFilePath;

    // Constructor initializes the log file path to the Downloads folder
    public ErrorLogger()
    {
        string desktopFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OLVimages");
        logFilePath = Path.Combine(desktopFolder, "OLVErrorLog.txt"); ;
    }

    // Method to log an error message with a timestamp
    public void LogError(string errorMessage)
    {
        try
        {
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(logFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Format the log entry with timestamp
            string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {errorMessage}{Environment.NewLine}";

            // Append the log entry to the file
           File.AppendAllText(logFilePath, logEntry);
        }
        catch (Exception ex)
        {
            // If logging fails, output to console to avoid infinite recursion
            Console.WriteLine($"Failed to log error: {ex.Message}");
        }
    }
}