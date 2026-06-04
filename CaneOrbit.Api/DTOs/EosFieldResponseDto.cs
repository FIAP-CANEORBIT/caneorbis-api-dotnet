namespace CaneOrbis.Api.DTOs
{
    public class EosFieldResponseDto
    {
        public int IdEosField { get; set; }

        public decimal? Area { get; set; }

        public string? Nome { get; set; }

        public DateTime? DataCriacao { get; set; }

        public string? Mensagem { get; set; }
    }
}