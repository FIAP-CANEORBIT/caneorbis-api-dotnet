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

            var propriedadeExiste = await _context.Propriedades
                .AnyAsync(p => p.IdPropriedade == dto.IdPropriedade);

            if (!propriedadeExiste)
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
    }
}