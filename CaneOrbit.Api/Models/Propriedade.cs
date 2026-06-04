namespace CaneOrbis.Api.Models
{
    public class Propriedade
    {
        public int IdPropriedade { get; set; }

        public int IdUsuario { get; set; }

        public string NmPropriedade { get; set; } = string.Empty;

        public string? DsLocalizacao { get; set; }

        public decimal? VlAreaHectare { get; set; }

        public Usuario? Usuario { get; set; }

        public ICollection<Field> Fields { get; set; } = new List<Field>();
    }
}