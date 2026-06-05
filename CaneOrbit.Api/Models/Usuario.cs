namespace CaneOrbis.Api.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public string NmUsuario { get; set; } = string.Empty;

        public string DsEmail { get; set; } = string.Empty;

        public string DsSenhaHash { get; set; } = string.Empty;

        public DateTime DtCadastro { get; set; } = DateTime.Now;

        public ICollection<Propriedade> Propriedades { get; set; } = new List<Propriedade>();
    }
}