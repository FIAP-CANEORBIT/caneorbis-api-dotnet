namespace CaneOrbis.Api.DTOs
{
    public class DadoSateliteCreateDto
    {
        public int IdDispositivo { get; set; }
        public decimal? VlNdvi { get; set; }
        public decimal? VlPrecipitacao { get; set; }
        public decimal? VlTemperaturaAr { get; set; }
        public string? DsCondicaoClima { get; set; }
    }
}