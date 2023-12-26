using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic.Logger
{
    public class Logger : ILogger
    {
        private readonly string logFilePath;
        public Logger(string filePath = "Log.txt")
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            logFilePath = Path.Combine(baseDirectory, filePath);
        }

        public void Log(string message)
        {
            string logMessage = $"{message}";

            try
            {
                if (!File.Exists(logFilePath))
                {
                    File.Create(logFilePath);
                }

                using (StreamWriter writer = File.AppendText(logFilePath))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
        public void LogError(Exception ex)
        {
            string fullMessage = "--------------------------------------------------";
            fullMessage += Environment.NewLine + $"Timestamp: {DateTime.Now}";
            fullMessage += Environment.NewLine + $"Exception Type: {this.GetType().FullName}";
            fullMessage += Environment.NewLine + $"Message: {ex.Message}";
            fullMessage += Environment.NewLine + $"Inner Exception: {ex.InnerException}";
            fullMessage += Environment.NewLine + $"Stack Trace: {ex.StackTrace}";
            fullMessage += Environment.NewLine + "--------------------------------------------------";

            Log(fullMessage);
        }


    }
}
