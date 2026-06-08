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
    public class FieldController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly EosService _eosService;

        public FieldController(AppDbContext context, EosService eosService)
        {
            _context = context;
            _eosService = eosService;
        }

        [HttpPost("criar-do-dispositivo/{idDispositivo}")]
        public async Task<IActionResult> CriarFieldDoDispositivo(
            int idDispositivo,
            FieldCreateDto dto)
        {
            var dispositivo = await _context.DispositivosIot
                .FirstOrDefaultAsync(d => d.IdDispositivo == idDispositivo);

            if (dispositivo == null)
                return NotFound("Dispositivo não encontrado.");

            if (dispositivo.IdField != null)
                return BadRequest("Este dispositivo já está vinculado a um Field.");

            if (dispositivo.VlLatitude == null || dispositivo.VlLongitude == null)
                return BadRequest("Dispositivo não possui latitude e longitude cadastradas.");

            var propriedade = await _context.Propriedades
    .FirstOrDefaultAsync(p => p.IdPropriedade == dto.IdPropriedade);

            if (propriedade == null)
                return BadRequest("Propriedade informada não existe.");

            var respostaEos = await _eosService.CriarFieldAsync(
                dispositivo.VlLatitude.Value,
                dispositivo.VlLongitude.Value,
                dto
            );

            if (respostaEos.IdEosField == 0)
                return BadRequest(respostaEos.Mensagem);

            var field = new Field
            {
                IdPropriedade = dto.IdPropriedade,
                IdEosField = respostaEos.IdEosField,
                NmField = dto.Nome,
                VlAreaHectare = respostaEos.Area
            };

            _context.Fields.Add(field);
            await _context.SaveChangesAsync();

            dispositivo.IdField = field.IdField;
            await _context.SaveChangesAsync();

            return Created(string.Empty, new
            {
                field.IdField,
                field.IdPropriedade,
                field.IdEosField,
                field.NmField,
                field.VlAreaHectare,
                field.DtCriacao,
                dispositivo.IdDispositivo
            });
        }

        [HttpGet("por-dispositivo/{idDispositivo}")]
        public async Task<IActionResult> GetFieldPorDispositivo(int idDispositivo)
        {
            var dispositivo = await _context.DispositivosIot
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.IdDispositivo == idDispositivo);

            if (dispositivo == null)
                return NotFound("Dispositivo não encontrado.");

            if (dispositivo.IdField == null)
                return NotFound("Este dispositivo não está vinculado a nenhum Field.");

            var field = await _context.Fields
                .AsNoTracking()
                .FirstOrDefaultAsync(f => f.IdField == dispositivo.IdField.Value);

            if (field == null)
                return NotFound("Field vinculado ao dispositivo não foi encontrado.");

            return Ok(new
            {
                field.IdField,
                field.IdPropriedade,
                field.IdEosField,
                field.NmField,
                field.VlAreaHectare,
                field.DtCriacao,
                dispositivo.IdDispositivo
            });
        }
    }
}