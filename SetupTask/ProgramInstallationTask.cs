using System.IO;
using SettingUpSystem.Services;

namespace SettingUpSystem.SetupTask
{
    internal class ProgramInstallationTask
    {
        public string Name => "Program installing";

        private readonly ProcessService _processService;

        public ProgramInstallationTask(ProcessService processService)
        {
            _processService = processService;
        }

        public async Task<bool> ExecuteAsync(ILogger logger)
        {
            string basePath = @"D:\(01) Soft_For_Shop";

            var installers = new List<(string fileName, string arguments)>
            {
            ("AnyDesk_6_2_3_0.exe", "/silent"),
            ("ChromeSetup.exe", "/silent /install"),
            ("LibreOffice_7.6.7_Win_x86-64.msi", "/quiet"),
            ("winrar-x64-701uk.exe", "/S")
            };

            bool allSucceeded = true;

            foreach (var (fileName, args) in installers)
            {
                string fullPath = Path.Combine(basePath, fileName);
                logger.Log($"Installing {fileName}...");

                if (!File.Exists(fullPath))
                {
                    logger.Log($"File not found {fullPath}");
                    allSucceeded = false;
                    continue;
                }

                bool result = _processService.RunSilentInstaller(fullPath, args);

                if (result)
                    logger.Log($"Success installed {fileName}");
                else
                {
                    logger.Log($"Error with {fileName}");
                    allSucceeded = false;
                }
            }
            return allSucceeded;
        }
    }
}
