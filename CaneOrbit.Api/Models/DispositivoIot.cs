namespace CaneOrbis.Api.Models
{
    public class DispositivoIot
    {
        public int IdDispositivo { get; set; }

        public int? IdField { get; set; }

        public string DsMacAddress { get; set; } = string.Empty;

        public string? NmApelido { get; set; }

        public decimal? VlLatitude { get; set; }

        public decimal? VlLongitude { get; set; }

        public string DsStatusDispositivo { get; set; } = "ATIVO";

        public DateTime DtInstalacao { get; set; } = DateTime.Now;

        public Field? Field { get; set; }

        public ICollection<LeituraSensor> LeiturasSensor { get; set; } = new List<LeituraSensor>();

        public ICollection<DadoSatelite> DadosSatelite { get; set; } = new List<DadoSatelite>();
    }
}