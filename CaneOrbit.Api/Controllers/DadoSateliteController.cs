using CaneOrbis.Api.Data;
using CaneOrbis.Api.DTOs;
using CaneOrbis.Api.Models;
using CaneOrbis.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaneOrbis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DadoSateliteController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EosService _eosService;

        public DadoSateliteController(AppDbContext context, EosService eosService)
        {
            _context = context;
            _eosService = eosService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DadoSatelite>>> GetDadosSatelite()
        {
            return await _context.DadosSatelite.AsNoTracking().ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DadoSatelite>> GetDadoSatelite(int id)
        {
            var dado = await _context.DadosSatelite
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDadoSatelite == id);

            if (dado == null)
                return NotFound();

            return dado;
        }

        [HttpPost]
        public async Task<ActionResult<DadoSatelite>> CriarDadoSatelite(DadoSateliteCreateDto dto)
        {
            var dispositivo = await _context.DispositivosIot
                .FirstOrDefaultAsync(d => d.IdDispositivo == dto.IdDispositivo);

            if (dispositivo == null)
                return BadRequest("Dispositivo informado não existe.");

            var dado = new DadoSatelite
            {
                IdDispositivo = dto.IdDispositivo,
                VlNdvi = dto.VlNdvi,
                VlPrecipitacao = dto.VlPrecipitacao,
                VlTemperaturaAr = dto.VlTemperaturaAr,
                DsCondicaoClima = dto.DsCondicaoClima
            };

            _context.DadosSatelite.Add(dado);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDadoSatelite), new { id = dado.IdDadoSatelite }, new
            {
                dado.IdDadoSatelite,
                dado.IdDispositivo,
                dado.VlNdvi,
                dado.VlPrecipitacao,
                dado.VlTemperaturaAr,
                dado.DsCondicaoClima,
                dado.DtColeta
            });
        }

        [HttpPost("coletar/{idDispositivo}")]
        public async Task<ActionResult<DadoSatelite>> ColetarDadoSatelite(int idDispositivo)
        {
            var dispositivo = await _context.DispositivosIot
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDispositivo == idDispositivo);

            if (dispositivo == null)
                return NotFound("Dispositivo não encontrado.");

            if (dispositivo.VlLatitude == null || dispositivo.VlLongitude == null)
                return BadRequest("Dispositivo não possui latitude e longitude cadastradas.");

            if (dispositivo.IdField == null)
                return BadRequest("Dispositivo ainda não está vinculado a um Field.");

            var field = await _context.Fields
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IdField == dispositivo.IdField.Value);

            if (field == null)
                return BadRequest("Field vinculado ao dispositivo não foi encontrado.");

            if (field.IdEosField == null)
                return BadRequest("Field vinculado ao dispositivo não possui ID da EOS.");

            var resultadoNdvi = await _eosService.BuscarNdviAsync(
                dispositivo.VlLatitude.Value,
                dispositivo.VlLongitude.Value
            );

            if (resultadoNdvi.Ndvi == null)
                return BadRequest(resultadoNdvi.Mensagem);

            var resultadoWeather = await _eosService.BuscarWeatherAsync(field.IdEosField.Value);

            if (resultadoWeather.Precipitacao == null &&
                resultadoWeather.TemperaturaAr == null &&
                string.IsNullOrWhiteSpace(resultadoWeather.CondicaoClima))
            {
                return BadRequest(resultadoWeather.Mensagem);
            }

            var dadoSatelite = new DadoSatelite
            {
                IdDispositivo = idDispositivo,
                VlNdvi = resultadoNdvi.Ndvi,
                VlPrecipitacao = resultadoWeather.Precipitacao,
                VlTemperaturaAr = resultadoWeather.TemperaturaAr,
                DsCondicaoClima = resultadoWeather.CondicaoClima
            };

            _context.DadosSatelite.Add(dadoSatelite);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDadoSatelite), new { id = dadoSatelite.IdDadoSatelite }, new
            {
                dadoSatelite.IdDadoSatelite,
                dadoSatelite.IdDispositivo,
                dadoSatelite.VlNdvi,
                dadoSatelite.VlPrecipitacao,
                dadoSatelite.VlTemperaturaAr,
                dadoSatelite.DsCondicaoClima,
                dadoSatelite.DtColeta
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarDadoSatelite(int id, DadoSateliteUpdateDto dto)
        {
            var dado = await _context.DadosSatelite.FindAsync(id);

            if (dado == null)
                return NotFound();

            dado.VlNdvi = dto.VlNdvi;
            dado.VlPrecipitacao = dto.VlPrecipitacao;
            dado.VlTemperaturaAr = dto.VlTemperaturaAr;
            dado.DsCondicaoClima = dto.DsCondicaoClima;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarDadoSatelite(int id)
        {
            var dado = await _context.DadosSatelite.FindAsync(id);

            if (dado == null)
                return NotFound();

            _context.DadosSatelite.Remove(dado);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("por-dispositivo/{idDispositivo}")]
        public async Task<IActionResult> GetDadosSatelitePorDispositivo(int idDispositivo)
        {
            var dadosSatelite = await _context.DadosSatelite
                .AsNoTracking()
                .Where(d => d.IdDispositivo == idDispositivo)
                .ToListAsync();

            if (dadosSatelite == null || !dadosSatelite.Any())
                return NotFound("Nenhuma leitura de satélite encontrada para este dispositivo.");

            return Ok(dadosSatelite);
        }

    }
}