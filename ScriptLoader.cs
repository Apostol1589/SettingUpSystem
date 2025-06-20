using System.IO;

namespace SettingUpSystem
{
    public static class ScriptLoader
    {
        public static string LoadScriptAsSingleLine(string fileName)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string scriptPath = Path.Combine(baseDir, "scripts", fileName);

            if (!File.Exists(scriptPath))
                throw new FileNotFoundException($"Script file not found");

            var lines = File.ReadAllLines(scriptPath);
            var oneLineScript = string.Join(" ", lines
                .Select(line => line.TrimEnd())
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    string trimmed = line.Trim();
                    if (!trimmed.EndsWith(";") && !trimmed.EndsWith("{") && !trimmed.EndsWith("}"))
                        trimmed += ";";
                    return trimmed;
                }));
            return oneLineScript;
        }
    }
}
