using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Management.Automation;

namespace SettingUpSystem
{
    internal class UserCreation
    {
        public static void CreateUserWithRDSSettings(
                string username,
                string password,
                string startProgramPath = null,
                string startInPath = null,
                bool connectClientDrives = true,
                bool connectClientPrinters = true,
                bool defaultToMainClientPrinter = true,
                int? activeSessionLimitDays = null,
                int? idleSessionLimitMinutes = null,
                bool disconnectWhenLimitReached = true,
                bool endSessionWhenLimitReached = false,
                bool allowReconnectionFromAnyClient = true)
        {
            try
            {
                CreateLocalUser(username, password);

                AddUserToRemoteDesktopGroup(username);

                if (!string.IsNullOrEmpty(startProgramPath))
                {
                    SetRDSEnvironmentSettings(
                        username,
                        startProgramPath,
                        startInPath,
                        connectClientDrives,
                        connectClientPrinters,
                        defaultToMainClientPrinter);
                }

                SetRDSSessionSettings(
                    username,
                    activeSessionLimitDays,
                    idleSessionLimitMinutes,
                    disconnectWhenLimitReached,
                    endSessionWhenLimitReached,
                    allowReconnectionFromAnyClient);

                Console.WriteLine($"Користувача {username} успішно створено з налаштованими параметрами RDS");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
        private static void SetRDSEnvironmentSettings(
            string username,
            string startProgramPath,
            string startInPath,
            bool connectClientDrives,
            bool connectClientPrinters,
            bool defaultToMainClientPrinter)
        {
            string script = $@"
                Import-Module RemoteDesktop
                $user = Get-LocalUser -Name '{username}'
                $params = @{{
                    InitialProgram = '{startProgramPath}'
                    WorkDirectory = '{startInPath}'
                    ConnectClientDrivesAtLogon = ${connectClientDrives.ToString().ToLower()}
                    ConnectClientPrintersAtLogon = ${connectClientPrinters.ToString().ToLower()}
                    DefaultToMainPrinter = ${defaultToMainClientPrinter.ToString().ToLower()}
                }}
                Set-RDUserEnvironment -User $user.SID @params
            ";

            ExecutePowerShellScript(script);
        }


        private static void SetRDSSessionSettings(
                    string username,
                    int? activeSessionLimitDays,
                    int? idleSessionLimitMinutes,
                    bool disconnectWhenLimitReached,
                    bool endSessionWhenLimitReached,
                    bool allowReconnectionFromAnyClient)
        {
            string script = $@"
                Import-Module RemoteDesktop
                $user = Get-LocalUser -Name '{username}'
                $params = @{{";

            if (activeSessionLimitDays.HasValue)
                script += $"ActiveSessionLimit = New-TimeSpan -Days {activeSessionLimitDays.Value}\r\n";

            if (idleSessionLimitMinutes.HasValue)
                script += $"IdleSessionLimit = New-TimeSpan -Minutes {idleSessionLimitMinutes.Value}\r\n";

            script += $@"
                    DisconnectedSessionLimit = New-TimeSpan -Days 1
                    BrokenConnectionAction = '{(disconnectWhenLimitReached ? "Disconnect" : "Terminate")}'
                    ReconnectionAction = '{(allowReconnectionFromAnyClient ? "FromAnyClient" : "FromOriginalClient")}'
                }}
                Set-RDUserSessionConfiguration -User $user.SID @params
            ";

            ExecutePowerShellScript(script);
        }

        private static void CreateLocalUser(string username, string password)
        {
            ExecuteCommand($"net user {username} {password} /add");
        }

        private static void AddUserToRemoteDesktopGroup(string username)
        {
            ExecuteCommand($"net localgroup \"Remote Desktop Users\" {username} /add");
        }

        private static void ExecuteCommand(string command)
        {
            ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", $"/c {command}")
            {
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true,
            };

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"Command '{command}' finished with error: {process.ExitCode}");
                }
            }
        }

        private static void ExecutePowerShellScript(string script)
        {
            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddScript(script);
                Collection<PSObject> result = ps.Invoke();
                
            }
        }

        public static void CreateUserSimple(string username, string password)
        {
            try
            {
                ExecuteCommand($"net user {username} {password} /add /passwordchg:no");
                Console.WriteLine($"Користувача {username} успішно створено");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}