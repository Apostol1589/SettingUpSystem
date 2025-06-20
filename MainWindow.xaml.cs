using System.Windows;
using System.Windows.Controls;

namespace SettingUpSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Clear();
            Log("Початок виконання скриптів...");

            var tasks = new List<(CheckBox, string)>
            {
                (GetCheckBoxByContent("Змінити часовий пояс"), "DateTime.ps1"),
                (GetCheckBoxByContent("Додати користувачів"), "Users.ps1"),
                (GetCheckBoxByContent("Налаштування приватності"), "Privacy.ps1"),
                (GetCheckBoxByContent("Встановити WinRAR"), "Winrar.ps1"),
                (GetCheckBoxByContent("Встановити Chrome"), "Chrome.ps1"),
                (GetCheckBoxByContent("Встановити Uniscale"), "Scales.ps1"),
                (GetCheckBoxByContent("Встановити RustDesk"), "Rust.ps1"),
                (GetCheckBoxByContent("Налаштувати завдання"), "SchedulerSetup.ps1"),
                (GetCheckBoxByContent("Підготувати папки"), "Folders.ps1"),
            };

            foreach (var (checkBox, scriptName) in tasks)
            {
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    Log($"Виконується: {checkBox.Content}...");

                    try
                    {
                        string script = ScriptLoader.LoadScriptAsSingleLine(scriptName);
                        string result = await Task.Run(() => PowerShellHendler.Command(script));
                        Log(result);
                    }
                    catch (Exception ex)
                    {
                        Log($"Помилка: {ex.Message}");
                    }
                }
            }

            Log("Виконання завершено.");
        }

        private CheckBox GetCheckBoxByContent(string content)
        {
            foreach (var child in ((Grid)((Grid)Content).Children[0]).Children)
            {
                if (child is CheckBox cb && cb.Content.ToString() == content)
                {
                    return cb;
                }
            }
            return null;
        }

        private void Log(string message)
        {
            Dispatcher.Invoke(() =>
            {
                LogTextBox.AppendText($"{message}\n");
                LogTextBox.ScrollToEnd();
            });
        }
    }
}
