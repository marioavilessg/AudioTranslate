using Maui2.Models;
using RabbitMQ.Client;
using System.Text;

namespace Maui2.Services;

public class RabbitMqService
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly ConfigurationService _configService;

    private bool _isConnecting;

    public bool IsConnected { get; private set; }
    public string? ConnectionError { get; private set; }

    public event Action? OnConnectionStateChanged;

    public RabbitMqService(ConfigurationService configService)
    {
        _configService = configService;
        _configService.OnSettingsChanged += async () => await ReconnectAsync();
    }

    public async Task ConnectAsync()
    {
        await TryConnectAsync(_configService.CurrentSettings);
    }

    private async Task ReconnectAsync()
    {
        Disconnect();
        await TryConnectAsync(_configService.CurrentSettings);
    }

    private async Task TryConnectAsync(RabbitSettings settings)
    {
        if (_isConnecting) return;

        _isConnecting = true;

        try
        {
            await Task.Run(() =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = settings.Host,
                    Port = settings.Port,
                    UserName = settings.Username,
                    Password = settings.Password,
                    AutomaticRecoveryEnabled = true,
                    NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare(settings.MyQueue, false, false, false);
                _channel.QueueDeclare(settings.TargetQueue, false, false, false);
            });

            IsConnected = true;
            ConnectionError = null;
        }
        catch (Exception ex)
        {
            IsConnected = false;
            ConnectionError = ex.Message;
        }

        _isConnecting = false;

        OnConnectionStateChanged?.Invoke();
    }

    public void Disconnect()
    {
        try
        {
            _channel?.Close();
            _connection?.Close();
        }
        catch { }

        _channel = null;
        _connection = null;
        IsConnected = false;
    }
}