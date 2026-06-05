namespace CaneOrbis.Api.DTOs
{
    public class FieldCreateDto
    {
        public int IdPropriedade { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string? Grupo { get; set; }

        public string? Cultura { get; set; }

        public int? Ano { get; set; }

        public DateTime? DataPlantio { get; set; }
    }
}