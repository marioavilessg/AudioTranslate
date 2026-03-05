namespace Maui1.Models;

public class ChatMessage
{
    public string SenderId { get; set; }

    public string OriginalText { get; set; }

    public string SourceLanguage { get; set; }

    public DateTime Timestamp { get; set; }
    
    public bool IsAudio { get; set; }

    public byte[]? AudioData { get; set; }

    public int AudioDuration { get; set; }
}