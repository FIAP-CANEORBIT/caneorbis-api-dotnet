using CaneOrbis.Api.DTOs;
using CaneOrbis.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CaneOrbis.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnaliseAgricolaController : ControllerBase
    {
        private readonly AnaliseAgricolaService _analiseAgricolaService;

        public AnaliseAgricolaController(AnaliseAgricolaService analiseAgricolaService)
        {
            _analiseAgricolaService = analiseAgricolaService;
        }

        [HttpGet("dispositivo/{idDispositivo}")]
        public async Task<ActionResult<AnaliseAgricolaResponseDto>> GerarAnalisePorDispositivo(int idDispositivo)
        {
            try
            {
                var resultado = await _analiseAgricolaService.GerarAnalisePorDispositivoAsync(idDispositivo);

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}