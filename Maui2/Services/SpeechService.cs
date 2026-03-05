using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;

namespace Maui2.Services;

public class SpeechService
{
    private readonly string _key;
    private readonly string _region;

    public SpeechService(IConfiguration config)
    {
        _key = config["AzureSpeech:Key"];
        _region = config["AzureSpeech:Region"];
    }

    // VOZ → TEXTO
    public async Task<string?> SpeechToTextAsync(string language)
    {
        var speechConfig = SpeechConfig.FromSubscription(_key, _region);
        speechConfig.SpeechRecognitionLanguage = MapLanguage(language);

        using var recognizer = new SpeechRecognizer(speechConfig);

        var result = await recognizer.RecognizeOnceAsync();

        if (result.Reason == ResultReason.RecognizedSpeech)
            return result.Text;

        return null;
    }

    // TEXTO → AUDIO
    public async Task<(byte[] Audio, int Duration)> TextToSpeechAsync(string text, string language)
    {
        var config = SpeechConfig.FromSubscription(_key, _region);

        config.SpeechSynthesisVoiceName = MapVoice(language);

        using var synthesizer = new SpeechSynthesizer(config, null);

        var result = await synthesizer.SpeakTextAsync(text);

        if (result.Reason == ResultReason.SynthesizingAudioCompleted)
        {
            int durationSeconds = (int)result.AudioDuration.TotalSeconds;

            return (result.AudioData, durationSeconds);
        }

        return (Array.Empty<byte>(), 0);
    }

    private string MapLanguage(string lang)
    {
        return lang switch
        {
            "es" => "es-ES",
            "en" => "en-US",
            "fr" => "fr-FR",
            "de" => "de-DE",
            _ => "en-US"
        };
    }

    private string MapVoice(string lang)
    {
        return lang switch
        {
            "es" => "es-ES-ElviraNeural",
            "en" => "en-US-JennyNeural",
            "fr" => "fr-FR-DeniseNeural",
            "de" => "de-DE-KatjaNeural",
            _ => "en-US-JennyNeural"
        };
    }
}