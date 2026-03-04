using Maui1.Services;

namespace Maui1;

public partial class App : Application
{
    private readonly ConfigurationService _config;
    private readonly RabbitMqService _rabbit;

    public App(ConfigurationService config, RabbitMqService rabbit)
    {
        InitializeComponent();

        _config = config;
        _rabbit = rabbit;

        _config.LoadDefaults();
        
        Task.Run(async () =>
        {
            await _rabbit.ConnectAsync();
        });
    }
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "Maui1" };
    }

}