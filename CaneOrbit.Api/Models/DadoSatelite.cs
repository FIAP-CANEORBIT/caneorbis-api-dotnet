namespace CaneOrbis.Api.Models
{
    public class DadoSatelite
    {
        public int IdDadoSatelite { get; set; }

        public int IdDispositivo { get; set; }

        public decimal? VlNdvi { get; set; }

        public decimal? VlPrecipitacao { get; set; }

        public decimal? VlTemperaturaAr { get; set; }

        public string? DsCondicaoClima { get; set; }

        public DateTime DtColeta { get; set; } = DateTime.Now;

        public DispositivoIot? DispositivoIot { get; set; }
    }
}