using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacientesController(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(PacienteDto pacienteDto)
        {
            await _pacienteRepository.CreateAsync(new Paciente()
            {
                Nombres = pacienteDto.Nombres,
                Apellidos = pacienteDto.Apellidos,
                DUI = pacienteDto.DUI,
                Telefono = pacienteDto.Telefono,
                CorreoElectronico = pacienteDto.CorreoElectronico
            });

            return Ok(pacienteDto);
        }

        [HttpGet("{skip}/{take}")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            var pacientes = await _pacienteRepository.GetAsync(skip, take);
            return Ok(pacientes);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paciente = await _pacienteRepository.GetById(id);

            if (paciente == null)
            {
                return NotFound();
            }

            return Ok(paciente);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _pacienteRepository.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PacienteDto pacienteDto)
        {
            var existingPaciente = await _pacienteRepository.GetById(id);
            if (existingPaciente == null)
            {
                return NotFound();
            }

            existingPaciente.Nombres = pacienteDto.Nombres;
            existingPaciente.Apellidos = pacienteDto.Apellidos;
            existingPaciente.DUI = pacienteDto.DUI;
            existingPaciente.Telefono = pacienteDto.Telefono;
            existingPaciente.CorreoElectronico = pacienteDto.CorreoElectronico;

            await _pacienteRepository.Update(existingPaciente);
            return Ok(existingPaciente);
        }
    }
}