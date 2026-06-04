namespace CaneOrbis.Api.DTOs
{
    public class AnaliseAgricolaResponseDto
    {
        public string Resumo { get; set; } = string.Empty;

        public string Alerta { get; set; } = string.Empty;

        public string NivelRisco { get; set; } = string.Empty;

        public string ExplicacaoSimples { get; set; } = string.Empty;

        public string RecomendacaoPratica { get; set; } = string.Empty;

        public string ConfiancaAnalise { get; set; } = string.Empty;
    }
}