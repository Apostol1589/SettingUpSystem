using SettingUpSystem.Services;

namespace SettingUpSystem.SetupTask
{
    internal class PrivacySetupTask
    {
        public string Name => "Privacy setup";

        public async Task<bool> ExecuteAsync(ILogger logger)
        {
            try
            {
                logger.Log("Privacy setup started...");

                var registryEdits = new List<RegistryEdit>
            {
                new(@"HKCU\Control Panel\International\User Profile", "HttpAcceptLanguageOptOut", 1),
                new(@"HKCU\Software\Microsoft\Windows\CurrentVersion\AdvertisingInfo", "Enabled", 0),
                new(@"HKCU\Control Panel\International\User Profile", "EnableInputPersonalization", 0),
                new(@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection", "AllowTelemetry", 1),
                new(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0),
                new(@"HKCU\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "OnlineSpeechAllowed", 0),
                new(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "PublishUserActivities", 0),
                new(@"HKLM\SOFTWARE\Policies\Microsoft\Windows\System", "UploadUserActivities", 0),
                new(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1),
                new(@"HKCU\Software\Microsoft\InputPersonalization", "RestrictImplicitInkCollection", 1),
                new(@"HKCU\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Start_TrackProgs", 0),
                new(@"HKLM\SOFTWARE\Microsoft\PolicyManager\default\Privacy\PublishUserActivities", "value", 0)
            };

                foreach (var edit in registryEdits)
                {
                    bool success = RegistryHelper.SetRegistryValue(edit.Path, edit.Name, edit.Value);
                    logger.Log(success
                        ? $"Istalled: [{edit.Path}] {edit.Name} = {edit.Value}"
                        : $"Error: [{edit.Path}] {edit.Name}");
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Log("Exception: " + ex.Message);
                return false;
            }
        }
    }
}
