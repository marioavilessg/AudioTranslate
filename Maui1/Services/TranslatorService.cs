using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace Maui1.Services;

public class TranslatorService
{
    private readonly HttpClient _httpClient = new();
    private readonly string _key;
    private readonly string _region;
    private readonly string _endpoint;

    public TranslatorService(IConfiguration config)
    {
        _key = config["AzureTranslator:Key"];
        _region = config["AzureTranslator:Region"];
        _endpoint = config["AzureTranslator:Endpoint"];
    }

    public async Task<string> TranslateAsync(string text, string from, string to)
    {
        string route = $"/translate?api-version=3.0&from={from}&to={to}";

        object[] body = { new { Text = text } };

        var requestBody = JsonSerializer.Serialize(body);

        using var request = new HttpRequestMessage(HttpMethod.Post, _endpoint + route);

        request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");

        request.Headers.Add("Ocp-Apim-Subscription-Key", _key);
        request.Headers.Add("Ocp-Apim-Subscription-Region", _region);

        var response = await _httpClient.SendAsync(request);

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);

        return doc.RootElement[0]
            .GetProperty("translations")[0]
            .GetProperty("text")
            .GetString()!;
    }
}