using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Win32;

namespace SettingUpSystem
{
    internal class UserCreation
    { 
        public static void CreateUser_Kasa(string username, string password, string startProgramPath, string startInPath)
        {
            try
            {
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = "net",
                        Arguments = $"user {username} {password} /add",
                        Verb = "runas",
                        UseShellExecute = true
                    }).WaitForExit();

                Process.Start(new ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = $"localgroup \"Remote Desktop Users\" {username} /add",
                    Verb = "runas",
                    UseShellExecute = true
                }).WaitForExit();

                Console.WriteLine($"User {username} created");

                string userRegistryPath = $"HKEY_USERS\\{GetUserSID(username)}\\Software\\Microsoft\\Windows\\CurrentVersion\\Run";
                Registry.SetValue(userRegistryPath, "Start1C", startProgramPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

        static string GetUserSID(string username)
        {
            var ntAccount = new NTAccount(username);
            var sid = (SecurityIdentifier)ntAccount.Translate(typeof(SecurityIdentifier));
            return sid.Value;
        }

        public static void CreateUser_simple(string username, string password)
        {
            string command = $"net user {username} {password} /add /passwordchg:no";

            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c" + command)
            {
                Verb = "runas",
                UseShellExecute = true,
            };

            try
            {
                Process process = Process.Start(psi);
                process.WaitForExit();

                Console.WriteLine("User created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: " + ex.Message);
            }
        }

    }
}
