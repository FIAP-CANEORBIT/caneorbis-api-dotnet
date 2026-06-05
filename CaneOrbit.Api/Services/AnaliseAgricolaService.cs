using CaneOrbis.Api.Data;
using CaneOrbis.Api.DTOs;
using CaneOrbit.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace CaneOrbis.Api.Services
{
    public class AnaliseAgricolaService
    {
        private readonly AppDbContext _context;
        private readonly GeminiService _geminiService;

        public AnaliseAgricolaService(AppDbContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        public async Task<AnaliseAgricolaResponseDto> GerarAnalisePorDispositivoAsync(int idDispositivo)
        {
            var dispositivo = await _context.DispositivosIot
                .AsNoTracking()
                .Include(d => d.Field)
                    .ThenInclude(f => f!.Propriedade)
                .FirstOrDefaultAsync(d => d.IdDispositivo == idDispositivo);

            if (dispositivo == null)
                throw new Exception("Dispositivo não encontrado.");

            var ultimaLeitura = await _context.LeiturasSensor
                .AsNoTracking()
                .Where(l => l.IdDispositivo == idDispositivo)
                .OrderByDescending(l => l.DtLeitura)
                .FirstOrDefaultAsync();

            var ultimoDadoSatelite = await _context.DadosSatelite
                .AsNoTracking()
                .Where(d => d.IdDispositivo == idDispositivo)
                .OrderByDescending(d => d.DtColeta)
                .FirstOrDefaultAsync();

            var prompt = MontarPrompt(dispositivo, ultimaLeitura, ultimoDadoSatelite);

            var respostaGemini = await _geminiService.GerarAnaliseAsync(prompt);

            return ConverterRespostaGeminiParaDto(respostaGemini);
        }

        private AnaliseAgricolaResponseDto ConverterRespostaGeminiParaDto(string respostaGemini)
        {
            try
            {
                var jsonLimpo = respostaGemini
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                var resultado = JsonSerializer.Deserialize<AnaliseAgricolaResponseDto>(
                    jsonLimpo,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                if (resultado == null)
                    throw new Exception("A resposta da IA veio vazia.");

                return resultado;
            }
            catch
            {
                return new AnaliseAgricolaResponseDto
                {
                    Resumo = "Não foi possível converter a resposta da IA para o formato estruturado.",
                    Alerta = "Verifique a resposta bruta retornada pela IA.",
                    NivelRisco = "Indefinido",
                    ExplicacaoSimples = respostaGemini,
                    RecomendacaoPratica = "Ajustar o prompt ou tentar gerar a análise novamente.",
                    ConfiancaAnalise = "Baixa"
                };
            }
        }

        private string MontarPrompt(
            Models.DispositivoIot dispositivo,
            Models.LeituraSensor? ultimaLeitura,
            Models.DadoSatelite? ultimoDadoSatelite)
        {
            var field = dispositivo.Field;
            var propriedade = field?.Propriedade;

            var prompt = new StringBuilder();

            prompt.AppendLine("Você é uma IA de apoio à análise agrícola.");
            prompt.AppendLine("O sistema é focado no monitoramento de cana-de-açúcar.");
            prompt.AppendLine();
            prompt.AppendLine("Analise os dados de IoT, satélite e clima para gerar uma recomendação simples.");
            prompt.AppendLine();
            prompt.AppendLine("Regras importantes:");
            prompt.AppendLine("- Não apresente diagnóstico agronômico definitivo.");
            prompt.AppendLine("- Use linguagem cautelosa.");
            prompt.AppendLine("- Use expressões como: 'os dados sugerem', 'pode indicar', 'há indício de', 'recomenda-se verificar'.");
            prompt.AppendLine("- Se houver ausência de dados, reduza a confiança da análise.");
            prompt.AppendLine("- Considere que NDVI baixo pode indicar baixo vigor vegetativo.");
            prompt.AppendLine("- Considere que precipitação zero e baixa umidade do solo podem indicar risco hídrico.");
            prompt.AppendLine("- Considere que pH muito baixo ou muito alto pode indicar necessidade de análise de solo.");
            prompt.AppendLine("- Considere que temperatura elevada pode aumentar o estresse da cultura.");
            prompt.AppendLine();
            prompt.AppendLine("Responda exclusivamente em JSON válido.");
            prompt.AppendLine("Não use markdown.");
            prompt.AppendLine("Não use ```json.");
            prompt.AppendLine("Não escreva nenhum texto fora do JSON.");
            prompt.AppendLine();
            prompt.AppendLine("Use exatamente esta estrutura:");
            prompt.AppendLine("{");
            prompt.AppendLine("  \"resumo\": \"texto do resumo\",");
            prompt.AppendLine("  \"alerta\": \"texto do alerta\",");
            prompt.AppendLine("  \"nivelRisco\": \"Baixo, Médio ou Alto\",");
            prompt.AppendLine("  \"explicacaoSimples\": \"explicação simples\",");
            prompt.AppendLine("  \"recomendacaoPratica\": \"recomendação prática\",");
            prompt.AppendLine("  \"confiancaAnalise\": \"Baixa, Média ou Alta\"");
            prompt.AppendLine("}");
            prompt.AppendLine();

            prompt.AppendLine("Dados disponíveis:");
            prompt.AppendLine();

            prompt.AppendLine("Dispositivo IoT:");
            prompt.AppendLine($"- ID do dispositivo: {dispositivo.IdDispositivo}");
            prompt.AppendLine($"- Apelido: {dispositivo.NmApelido ?? "Não informado"}");
            prompt.AppendLine($"- Status: {dispositivo.DsStatusDispositivo}");
            prompt.AppendLine($"- Latitude: {dispositivo.VlLatitude?.ToString() ?? "Não informada"}");
            prompt.AppendLine($"- Longitude: {dispositivo.VlLongitude?.ToString() ?? "Não informada"}");
            prompt.AppendLine();

            prompt.AppendLine("Field:");
            prompt.AppendLine($"- Field vinculado: {(field != null ? "Sim" : "Não")}");
            prompt.AppendLine($"- Nome do Field: {field?.NmField ?? "Não informado"}");
            prompt.AppendLine($"- Área do Field em hectares: {field?.VlAreaHectare?.ToString() ?? "Não informada"}");
            prompt.AppendLine($"- ID EOS Field: {field?.IdEosField?.ToString() ?? "Não informado"}");
            prompt.AppendLine();

            prompt.AppendLine("Propriedade:");
            prompt.AppendLine($"- Nome: {propriedade?.NmPropriedade ?? "Não informada"}");
            prompt.AppendLine($"- Localização: {propriedade?.DsLocalizacao ?? "Não informada"}");
            prompt.AppendLine($"- Área em hectares: {propriedade?.VlAreaHectare?.ToString() ?? "Não informada"}");
            prompt.AppendLine();

            prompt.AppendLine("Última leitura do sensor:");
            if (ultimaLeitura == null)
            {
                prompt.AppendLine("- Nenhuma leitura de sensor encontrada para este dispositivo.");
            }
            else
            {
                prompt.AppendLine($"- ID da leitura: {ultimaLeitura.IdLeitura}");
                prompt.AppendLine($"- Umidade do solo: {ultimaLeitura.VlUmidadeSolo}");
                prompt.AppendLine($"- Temperatura do sensor: {ultimaLeitura.VlTemperatura}");
                prompt.AppendLine($"- pH do solo: {ultimaLeitura.VlPhSolo?.ToString() ?? "Não informado"}");
                prompt.AppendLine($"- Data da leitura: {ultimaLeitura.DtLeitura}");
            }

            prompt.AppendLine();

            prompt.AppendLine("Último dado de satélite e clima:");
            if (ultimoDadoSatelite == null)
            {
                prompt.AppendLine("- Nenhum dado de satélite/clima encontrado para este dispositivo.");
            }
            else
            {
                prompt.AppendLine($"- ID do dado de satélite: {ultimoDadoSatelite.IdDadoSatelite}");
                prompt.AppendLine($"- NDVI: {ultimoDadoSatelite.VlNdvi?.ToString() ?? "Não informado"}");
                prompt.AppendLine($"- Precipitação: {ultimoDadoSatelite.VlPrecipitacao?.ToString() ?? "Não informada"}");
                prompt.AppendLine($"- Temperatura do ar: {ultimoDadoSatelite.VlTemperaturaAr?.ToString() ?? "Não informada"}");
                prompt.AppendLine($"- Condição climática: {ultimoDadoSatelite.DsCondicaoClima ?? "Não informada"}");
                prompt.AppendLine($"- Data da coleta: {ultimoDadoSatelite.DtColeta}");
            }

            return prompt.ToString();
        }
    }
}