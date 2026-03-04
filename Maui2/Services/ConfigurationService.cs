using Maui2.Models;
using Microsoft.Maui.Storage;

namespace Maui2.Services;

public class ConfigurationService
{
    public RabbitSettings CurrentSettings { get; private set; } = new();

    public event Action? OnSettingsChanged;

    public void LoadDefaults()
    {
        CurrentSettings = new RabbitSettings
        {
            Host = "localhost",
            Port = 5672,
            Username = "admin",
            Password = "admin",
            MyQueue = "cola_B",
            TargetQueue = "cola_A"
        };
    }

    public void SaveToPreferences()
    {
        Preferences.Set("rabbit_host", CurrentSettings.Host ?? "");
        Preferences.Set("rabbit_port", CurrentSettings.Port);
        Preferences.Set("rabbit_user", CurrentSettings.Username ?? "");
        Preferences.Set("rabbit_pass", CurrentSettings.Password ?? "");
        Preferences.Set("rabbit_myqueue", CurrentSettings.MyQueue ?? "");
        Preferences.Set("rabbit_targetqueue", CurrentSettings.TargetQueue ?? "");
    }

    public void UpdateSettings(RabbitSettings settings)
    {
        CurrentSettings = settings;
        SaveToPreferences();
        OnSettingsChanged?.Invoke();
    }
}