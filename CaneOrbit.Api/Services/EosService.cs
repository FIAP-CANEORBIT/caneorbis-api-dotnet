using System.Text;
using System.Text.Json;
using CaneOrbis.Api.DTOs;
using CaneOrbis.Api.Models;
using Microsoft.Extensions.Options;

namespace CaneOrbis.Api.Services
{
    public class EosService
    {
        private readonly EosSettings _settings;
        private readonly HttpClient _httpClient;

        public EosService(IOptions<EosSettings> settings)
        {
            _settings = settings.Value;
            _httpClient = new HttpClient();
        }

        public async Task<EosNdviResponseDto> BuscarNdviAsync(decimal latitude, decimal longitude)
        {
            var taskId = await CriarTaskNdviAsync(latitude, longitude);

            if (string.IsNullOrWhiteSpace(taskId))
            {
                return new EosNdviResponseDto
                {
                    Ndvi = null,
                    Status = "erro",
                    Mensagem = "Não foi possível criar a tarefa na EOS."
                };
            }

            for (int tentativa = 1; tentativa <= 6; tentativa++)
            {
                await Task.Delay(10000);

                var resultado = await ConsultarResultadoNdviAsync(taskId);

                if (resultado.Status == "concluido" || resultado.Status == "sem_dados" || resultado.Status == "erro")
                    return resultado;
            }

            return new EosNdviResponseDto
            {
                Ndvi = null,
                Status = "processando",
                Mensagem = "A EOS criou a tarefa, mas o processamento ainda não foi concluído."
            };
        }

        private async Task<string?> CriarTaskNdviAsync(decimal latitude, decimal longitude)
        {
            var lat = (double)latitude;
            var lon = (double)longitude;

            var delta = 0.001;

            var body = new
            {
                type = "mt_stats",
                @params = new
                {
                    bm_type = new[] { "NDVI" },
                    date_start = DateTime.UtcNow.AddDays(-30).ToString("yyyy-MM-dd"),
                    date_end = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    geometry = new
                    {
                        type = "Polygon",
                        coordinates = new[]
                        {
                            new[]
                            {
                                new[] { lon - delta, lat - delta },
                                new[] { lon - delta, lat + delta },
                                new[] { lon + delta, lat + delta },
                                new[] { lon + delta, lat - delta },
                                new[] { lon - delta, lat - delta }
                            }
                        }
                    },
                    reference = $"caneorbis-{DateTime.UtcNow:yyyyMMddHHmmss}",
                    sensors = new[] { "sentinel2" }
                }
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var url = $"{_settings.BaseUrl}/api/gdw/api?api_key={_settings.ApiKey}";

            var response = await _httpClient.PostAsync(url, content);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                return null;

            using var document = JsonDocument.Parse(responseBody);

            if (document.RootElement.TryGetProperty("task_id", out var taskIdElement))
                return taskIdElement.GetString();

            return null;
        }

        private async Task<EosNdviResponseDto> ConsultarResultadoNdviAsync(string taskId)
        {
            var url = $"{_settings.BaseUrl}/api/gdw/api/{taskId}";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("x-api-key", _settings.ApiKey);

            var response = await _httpClient.SendAsync(request);
            var responseBody = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return new EosNdviResponseDto
                {
                    Ndvi = null,
                    Status = "erro",
                    Mensagem = responseBody
                };
            }

            using var document = JsonDocument.Parse(responseBody);

            if (!document.RootElement.TryGetProperty("result", out var resultElement) ||
                resultElement.GetArrayLength() == 0)
            {
                return new EosNdviResponseDto
                {
                    Ndvi = null,
                    Status = "processando",
                    Mensagem = "A EOS ainda não retornou resultado para esta tarefa."
                };
            }

            var primeiroResultado = resultElement[0];

            if (primeiroResultado.TryGetProperty("indexes", out var indexesElement) &&
                indexesElement.TryGetProperty("NDVI", out var ndviElement) &&
                ndviElement.TryGetProperty("average", out var averageElement))
            {
                return new EosNdviResponseDto
                {
                    Ndvi = averageElement.GetDecimal(),
                    Status = "concluido",
                    Mensagem = "NDVI obtido com sucesso."
                };
            }

            return new EosNdviResponseDto
            {
                Ndvi = null,
                Status = "sem_dados",
                Mensagem = "A resposta da EOS não retornou NDVI médio."
            };
        }
    }
}