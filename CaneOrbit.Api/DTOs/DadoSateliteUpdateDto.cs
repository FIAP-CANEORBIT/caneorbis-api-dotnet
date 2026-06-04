namespace CaneOrbis.Api.DTOs
{
    public class DadoSateliteUpdateDto
    {
        public decimal? VlNdvi { get; set; }
        public decimal? VlPrecipitacao { get; set; }
        public decimal? VlTemperaturaAr { get; set; }
        public string? DsCondicaoClima { get; set; }
    }
}