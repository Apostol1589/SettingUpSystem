using System.IO;

namespace SettingUpSystem
{
    public interface ILogger
    {
        void Log(string message);
    }

    public class FileLogger : ILogger
    {
        private readonly string _logPath;

        public FileLogger(string logPath)
        {
            _logPath = logPath;
        }

        public void Log(string message)
        {
            string entry = $"[{DateTime.Now:HH:mm:ss}] {message}";
            File.AppendAllLines(_logPath, new[] { entry });
            Console.WriteLine(entry);
        }

    }
}
