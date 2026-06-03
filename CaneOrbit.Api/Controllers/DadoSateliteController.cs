using CaneOrbis.Api.Data;
using CaneOrbis.Api.DTOs;
using CaneOrbis.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaneOrbis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DadoSateliteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DadoSateliteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DadoSatelite>>> GetDadosSatelite()
        {
            return await _context.DadosSatelite
                .AsNoTracking()
                .ToListAsync();
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
            var dispositivoExiste = await _context.DispositivosIot
                .AnyAsync(d => d.IdDispositivo == dto.IdDispositivo);

            if (!dispositivoExiste)
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

            return CreatedAtAction(
                nameof(GetDadoSatelite),
                new { id = dado.IdDadoSatelite },
                dado
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarDadoSatelite(
            int id,
            DadoSateliteUpdateDto dto)
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
    }
}