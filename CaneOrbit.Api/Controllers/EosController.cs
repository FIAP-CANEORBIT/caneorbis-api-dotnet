using CaneOrbis.Api.DTOs;
using CaneOrbis.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CaneOrbis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EosController : ControllerBase
    {
        private readonly EosService _eosService;

        public EosController(EosService eosService)
        {
            _eosService = eosService;
        }

        [HttpGet("ndvi")]
        public async Task<ActionResult<EosNdviResponseDto>> BuscarNdvi(
            decimal latitude,
            decimal longitude)
        {
            var resultado = await _eosService.BuscarNdviAsync(latitude, longitude);

            return Ok(resultado);
        }
    }
}