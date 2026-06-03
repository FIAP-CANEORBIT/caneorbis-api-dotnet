using CaneOrbis.Api.Data;
using CaneOrbis.Api.DTOs;
using CaneOrbis.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CaneOrbis.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeituraSensorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LeituraSensorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LeituraSensor>>> GetLeituras()
        {
            return await _context.LeiturasSensor
                .AsNoTracking()
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LeituraSensor>> GetLeitura(int id)
        {
            var leitura = await _context.LeiturasSensor
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.IdLeitura == id);

            if (leitura == null)
                return NotFound();

            return leitura;
        }

        [HttpPost]
        public async Task<ActionResult<LeituraSensor>> CriarLeitura(LeituraSensorCreateDto dto)
        {
            var dispositivoExiste = await _context.DispositivosIot
                .AnyAsync(d => d.IdDispositivo == dto.IdDispositivo);

            if (!dispositivoExiste)
                return BadRequest("Dispositivo informado não existe.");

            if (dto.VlUmidadeSolo < 0 || dto.VlTemperatura < 0)
                return BadRequest("Umidade e temperatura não podem ser negativas.");

            var leitura = new LeituraSensor
            {
                IdDispositivo = dto.IdDispositivo,
                VlUmidadeSolo = dto.VlUmidadeSolo,
                VlTemperatura = dto.VlTemperatura,
                VlPhSolo = dto.VlPhSolo
            };

            _context.LeiturasSensor.Add(leitura);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLeitura), new { id = leitura.IdLeitura }, leitura);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarLeitura(int id, LeituraSensorUpdateDto dto)
        {
            var leitura = await _context.LeiturasSensor.FindAsync(id);

            if (leitura == null)
                return NotFound();

            if (dto.VlUmidadeSolo < 0 || dto.VlTemperatura < 0)
                return BadRequest("Umidade e temperatura não podem ser negativas.");

            leitura.VlUmidadeSolo = dto.VlUmidadeSolo;
            leitura.VlTemperatura = dto.VlTemperatura;
            leitura.VlPhSolo = dto.VlPhSolo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarLeitura(int id)
        {
            var leitura = await _context.LeiturasSensor.FindAsync(id);

            if (leitura == null)
                return NotFound();

            _context.LeiturasSensor.Remove(leitura);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}