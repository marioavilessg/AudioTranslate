using Maui1.Models;
using RabbitMQ.Client;
using System.Text;

namespace Maui1.Services;

public class RabbitMqService
{
    private IConnection? _connection;
    private IModel? _channel;
    private readonly ConfigurationService _configService;

    private bool _isConnecting;

    public bool IsConnected { get; private set; }
    public string? ConnectionError { get; private set; }

    public event Action? OnConnectionStateChanged;
    public event Action<ChatMessage>? OnMessageReceived;

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
                var consumer = new RabbitMQ.Client.Events.EventingBasicConsumer(_channel);

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();

                    var json = Encoding.UTF8.GetString(body);

                    var message = System.Text.Json.JsonSerializer.Deserialize<ChatMessage>(json);

                    if (message != null)
                    {
                        OnMessageReceived?.Invoke(message);
                    }
                };

                _channel.BasicConsume(
                    queue: settings.MyQueue,
                    autoAck: true,
                    consumer: consumer
                );
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
    
    public void SendMessage(ChatMessage message)
    {
        if (!IsConnected || _channel == null) return;

        var json = System.Text.Json.JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(json);

        _channel.BasicPublish(
            exchange: "",
            routingKey: _configService.CurrentSettings.TargetQueue,
            basicProperties: null,
            body: body
        );
    }
}