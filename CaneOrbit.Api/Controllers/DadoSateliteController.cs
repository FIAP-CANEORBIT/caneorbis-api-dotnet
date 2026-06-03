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

            var resultadoEos = await _eosService.BuscarNdviAsync(
                dispositivo.VlLatitude.Value,
                dispositivo.VlLongitude.Value
            );

            if (resultadoEos.Ndvi == null)
                return BadRequest(resultadoEos.Mensagem);

            var dadoSatelite = new DadoSatelite
            {
                IdDispositivo = idDispositivo,
                VlNdvi = resultadoEos.Ndvi,
                VlPrecipitacao = null,
                VlTemperaturaAr = null,
                DsCondicaoClima = ClassificarVegetacao(resultadoEos.Ndvi.Value)
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

        private static string ClassificarVegetacao(decimal ndvi)
        {
            if (ndvi < 0) return "AGUA_OU_NUVEM";
            if (ndvi < 0.2m) return "SOLO_EXPOSTO";
            if (ndvi < 0.4m) return "VEGETACAO_FRACA";
            if (ndvi < 0.6m) return "VEGETACAO_MODERADA";
            if (ndvi < 0.8m) return "VEGETACAO_SAUDAVEL";

            return "VEGETACAO_DENSA";
        }
    }
}