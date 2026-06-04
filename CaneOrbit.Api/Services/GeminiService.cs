using System.Text;
using System.Text.Json;
using CaneOrbit.Api.Models;
using Microsoft.Extensions.Options;

namespace CaneOrbit.Api.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiSettings _settings;

        public GeminiService(HttpClient httpClient, IOptions<GeminiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<string> GerarAnaliseAsync(string prompt)
        {
            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
                throw new Exception("A chave da API do Gemini não foi configurada.");

            if (string.IsNullOrWhiteSpace(_settings.BaseUrl))
                throw new Exception("A BaseUrl do Gemini não foi configurada.");

            if (string.IsNullOrWhiteSpace(_settings.Model))
                throw new Exception("O modelo do Gemini não foi configurado.");

            var url = $"{_settings.BaseUrl}/models/{_settings.Model}:generateContent?key={_settings.ApiKey}";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                                text = prompt
                            }
                        }
                    }
                }
            };

            var json = JsonSerializer.Serialize(requestBody);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao chamar Gemini: {response.StatusCode} - {responseContent}");
            }

            using var document = JsonDocument.Parse(responseContent);

            var texto = document
                .RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return texto ?? "O Gemini não retornou uma análise.";
        }
    }
}
