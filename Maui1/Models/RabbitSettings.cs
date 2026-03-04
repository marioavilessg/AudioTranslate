namespace Maui1.Models;

public class RabbitSettings
{
    public string Host { get; set; }
    public int Port { get; set; } = 5672;
    public string Username { get; set; }
    public string Password { get; set; }
    public string MyQueue { get; set; }
    public string TargetQueue { get; set; }
}