using System.Management.Automation;
using System.Text;

namespace SettingUpSystem
{
    public static class PowerShellHendler
    {
        private static PowerShell ps = PowerShell.Create();

        public static string Command(string script)
        {
            string errorMessage = string.Empty;
            string output = string.Empty;

            ps.AddScript(script);

            ps.AddScript("Out-String");

            PSDataCollection<PSObject> outputCollection = new();
            ps.Streams.Error.DataAdded += (object sender, DataAddedEventArgs e) =>
            {
                errorMessage = ((PSDataCollection<ErrorRecord>)sender)[e.Index].ToString();
            };

            IAsyncResult result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);

            ps.EndInvoke(result);

            StringBuilder sb = new();

            foreach (var outputItem in outputCollection)
            {
                sb.AppendLine(outputItem.BaseObject.ToString());
            }

            ps.Commands.Clear();

            if (!string.IsNullOrEmpty(errorMessage))
                return errorMessage;

            return sb.ToString();
        }
    }
}
