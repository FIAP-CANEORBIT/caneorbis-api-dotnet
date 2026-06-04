namespace CaneOrbis.Api.DTOs
{
    public class EosWeatherResponseDto
    {
        public decimal? Precipitacao { get; set; }

        public decimal? TemperaturaAr { get; set; }

        public string? CondicaoClima { get; set; }

        public string? Mensagem { get; set; }
    }
}