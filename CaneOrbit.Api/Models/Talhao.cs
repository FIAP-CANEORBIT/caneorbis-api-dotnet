namespace CaneOrbis.Api.Models
{
    public class Talhao
    {
        public int IdTalhao { get; set; }

        public int IdPropriedade { get; set; }

        public string NmTalhao { get; set; } = string.Empty;

        public decimal? VlAreaHectare { get; set; }

        public string? DsTipoSolo { get; set; }

        public string DsCultura { get; set; } = "CANA_DE_ACUCAR";

        public Propriedade? Propriedade { get; set; }

        public ICollection<DispositivoIot> DispositivosIot { get; set; } = new List<DispositivoIot>();
    }
}