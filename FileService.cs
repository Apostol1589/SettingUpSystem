using System.IO;

namespace SettingUpSystem
{
    public class FileService
    {
        public void CreateFolder(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Console.WriteLine($"{path} exists already.");
                    return;
                }
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The process failed: {ex}");
            }
        }

        public void DeleteFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Console.WriteLine($"{path} not exist.");
                    return;
                }
                DirectoryInfo di = Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"The process failed: {ex}");
            }
        }

        public void CopyFolder(string pathFrom, string pathTo)
        {
            var dir = new DirectoryInfo(pathFrom);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(pathTo);

            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(pathTo, file.Name);
                file.CopyTo(targetFilePath);
            }
        }
    }
}
