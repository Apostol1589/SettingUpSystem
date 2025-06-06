using System.IO;

namespace SettingUpSystem.Services
{
    public class FileService
    {
        private readonly ILogger _logger;

        public FileService(ILogger logger)
        {
            _logger = logger;
        }

        public bool CopyDirectory(string sourceDir, string destDir, bool overwrite = true)
        {
            try
            {
                if (!Directory.Exists(sourceDir))
                {
                    _logger.Log($"Directory not exist: {sourceDir}");
                    return false;
                }

                Directory.CreateDirectory(destDir);

                foreach (var file in Directory.GetFiles(sourceDir))
                {
                    var fileName = Path.GetFileName(file);
                    var destFile = Path.Combine(destDir, fileName);
                    File.Copy(file, destFile, overwrite);
                }

                foreach (var dir in Directory.GetDirectories(sourceDir))
                {
                    var dirName = Path.GetFileName(dir);
                    var destSubDir = Path.Combine(destDir, dirName);
                    CopyDirectory(dir, destSubDir, overwrite);
                }

                _logger.Log($"Copy from '{sourceDir}' in '{destDir}' done.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error while directory copied: {ex.Message}");
                return false;
            }
        }

        public bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.Log($"Created directory: {path}");
            }
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}
