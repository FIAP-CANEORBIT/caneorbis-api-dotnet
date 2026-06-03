namespace CaneOrbis.Api.Models
{
    public class LeituraSensor
    {
        public int IdLeitura { get; set; }
        public int IdDispositivo { get; set; }
        public decimal VlUmidadeSolo { get; set; }
        public decimal VlTemperatura { get; set; }
        public decimal? VlPhSolo { get; set; }
        public DateTime DtLeitura { get; set; } = DateTime.Now;

        public DispositivoIot? DispositivoIot { get; set; }
    }
}