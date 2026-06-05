namespace CaneOrbis.Api.Models
{
    public class Field
    {
        public int IdField { get; set; }

        public int IdPropriedade { get; set; }

        public int? IdEosField { get; set; }

        public string NmField { get; set; } = string.Empty;

        public decimal? VlAreaHectare { get; set; }

        public DateTime DtCriacao { get; set; } = DateTime.Now;

        public Propriedade? Propriedade { get; set; }

        public ICollection<DispositivoIot> DispositivosIot { get; set; } = new List<DispositivoIot>();
    }
}