using System.Diagnostics;
using System.IO;
using SettingUpSystem.Services;

namespace SettingUpSystem.SetupTask
{
    public class TaskSchedulerSetupTask
    {
        public string Name => "Task Scheduler";

        private readonly FileService _fileService;

        public TaskSchedulerSetupTask(FileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<bool> ExecuteAsync(ILogger logger)
        {
            try
            {
                string sourceFolder = @"D:\(01) Soft_For_Shop\Podsobka\bsClr";
                string destinationFolder = @"C:\Windows\BsClr";

                
                bool copied = _fileService.CopyDirectory(sourceFolder, destinationFolder);
                if (!copied)
                {
                    logger.Log("Failed copy bsClr.");
                    return false;
                }

               
                string taskFolder = @"D:\(01) Soft_For_Shop\Podsobka";
                string[] taskFiles = { "BS_upd.xml", "BS_temp_clr.xml", "bs_DownloadUpload.xml" };

                foreach (var taskFile in taskFiles)
                {
                    string xmlPath = Path.Combine(taskFolder, taskFile);
                    if (!_fileService.FileExists(xmlPath))
                    {
                        logger.Log($"File not exist: {xmlPath}");
                        continue;
                    }

                    string taskName = Path.GetFileNameWithoutExtension(taskFile);
                    string powershellCommand = $"Register-ScheduledTask -Xml (Get-Content -Path '{xmlPath}' -Raw) -TaskName '{taskName}' -Force";

                    var result = await ExecutePowerShellAsync(powershellCommand, logger);
                    if (result)
                    {
                        logger.Log($"Task '{taskName}' imported.");
                    }
                    else
                    {
                        logger.Log($"Import failed: {taskName}");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Log($"Erorr Task Scheduler: {ex.Message}");
                return false;
            }
        }

        private Task<bool> ExecutePowerShellAsync(string command, ILogger logger)
        {
            return Task.Run(() =>
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(psi))
                    {
                        string output = process.StandardOutput.ReadToEnd();
                        string error = process.StandardError.ReadToEnd();
                        process.WaitForExit();

                        if (!string.IsNullOrWhiteSpace(output))
                            logger.Log($"PowerShell output: {output}");
                        if (!string.IsNullOrWhiteSpace(error))
                            logger.Log($"PowerShell error: {error}");

                        return process.ExitCode == 0;
                    }
                }
                catch (Exception ex)
                {
                    logger.Log($"Error while PowerShell command: {ex.Message}");
                    return false;
                }
            });
        }
    }
}
