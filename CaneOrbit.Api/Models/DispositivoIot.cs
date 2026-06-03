namespace CaneOrbis.Api.Models
{
    public class DispositivoIot
    {
        public int IdDispositivo { get; set; }

        public ICollection<LeituraSensor> LeiturasSensor { get; set; } = new List<LeituraSensor>();

        public ICollection<DadoSatelite> DadosSatelite { get; set; } = new List<DadoSatelite>();
    }
}