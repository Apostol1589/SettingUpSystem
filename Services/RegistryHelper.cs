using Microsoft.Win32;

namespace SettingUpSystem.Services
{
    internal class RegistryHelper
    {
        public static bool SetRegistryValue(string fullPath, string name, object value)
        {
            try
            {
                var (rootKey, subPath) = ParseRegistryPath(fullPath);

                using var key = RegistryKey.OpenBaseKey(rootKey, RegistryView.Registry64)
                                           .CreateSubKey(subPath, true);

                if (key == null) return false;

                key.SetValue(name, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static (RegistryHive hive, string subPath) ParseRegistryPath(string fullPath)
        {
            if (fullPath.StartsWith("HKLM"))
                return (RegistryHive.LocalMachine, fullPath[5..]);
            else if (fullPath.StartsWith("HKCU"))
                return (RegistryHive.CurrentUser, fullPath[5..]);
            else
                throw new ArgumentException("Unsupported registry root: " + fullPath);
        }
    }
}
