namespace CaneOrbis.Api.DTOs
{
    public class LeituraSensorCreateDto
    {
        public int IdDispositivo { get; set; }
        public decimal VlUmidadeSolo { get; set; }
        public decimal VlTemperatura { get; set; }
        public decimal? VlPhSolo { get; set; }
    }
}