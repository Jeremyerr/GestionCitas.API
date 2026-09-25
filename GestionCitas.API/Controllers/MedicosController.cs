using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicosController : ControllerBase
    {
        private readonly IMedicoRepository _medicoRepository;

        public MedicosController(IMedicoRepository medicoRepository)
        {
            _medicoRepository = medicoRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(MedicoDto medico)
        {
            await _medicoRepository.CreateAsync(new Medico()
            {
                Nombres = medico.Nombres,
                Apellidos = medico.Apellidos,
                NumeroJVPM = medico.NumeroJVPM,
                EspecialidadId = medico.EspecialidadId
            });

            return Ok(medico);
        }

        [HttpGet("{skip}/{take}")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            var medicos = await _medicoRepository.GetAsync(skip, take);
            return Ok(medicos);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Traemos los datos del Médico junto con su Especialidad
            var medico = await _medicoRepository.GetById(id, m => m.Especialidad);

            if (medico == null)
            {
                return NotFound();
            }

            return Ok(medico);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _medicoRepository.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MedicoDto medico)
        {
            var existingMedico = await _medicoRepository.GetById(id);
            if (existingMedico == null) return NotFound();

            existingMedico.Nombres = medico.Nombres;
            existingMedico.Apellidos = medico.Apellidos;
            existingMedico.NumeroJVPM = medico.NumeroJVPM; 
            existingMedico.EspecialidadId = medico.EspecialidadId;

            await _medicoRepository.Update(existingMedico);
            return Ok(existingMedico);
        }
    }
}