using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly ICitaRepository _citaRepository;
        private readonly IMedicoRepository _medicoRepository; // Inyectado para validar horario laboral

        // Agregamos IMedicoRepository al constructor
        public CitasController(ICitaRepository citaRepository, IMedicoRepository medicoRepository)
        {
            _citaRepository = citaRepository;
            _medicoRepository = medicoRepository;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Medico")]
        public async Task<IActionResult> Create(CitaDto cita)
        {
            // 1. Obtener datos del médico para revisar su horario
            var medico = await _medicoRepository.GetById(cita.MedicoId);
            if (medico == null)
            {
                return NotFound("El médico especificado no existe.");
            }

            // 2. Validar que la cita esté dentro del horario laboral del médico
            var horaCita = cita.FechaHora.TimeOfDay;
            if (horaCita < medico.HoraInicio || horaCita > medico.HoraFin)
            {
                return BadRequest($"La cita está fuera del horario laboral del médico. Horario de atención: {medico.HoraInicio} a {medico.HoraFin}");
            }

            // 3. Validar disponibilidad (citas conflictivas)
            var existeConflicto = await _citaRepository.ExisteCitaConflictivaAsync(cita.MedicoId, cita.FechaHora);
            if (existeConflicto)
            {
                return BadRequest("El médico ya tiene una cita programada para esa fecha y hora exacta.");
            }

            // 4. Si pasa las validaciones, creamos la cita
            await _citaRepository.CreateAsync(new Cita()
            {
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo
            });

            return Ok(cita);
        }

        [HttpGet("{skip}/{take}")]
        [Authorize(Roles = "Admin,Medico,Paciente")]
        public async Task<IActionResult> Get(int skip, int take)
        {
            var citas = await _citaRepository.GetAsync(skip, take);
            return Ok(citas);
        }

        [HttpGet("getById/{id}")]
        [Authorize(Roles = "Admin,Medico,Paciente")]
        public async Task<IActionResult> GetById(int id)
        {
            var cita = await _citaRepository.GetById(id, c => c.Paciente, c => c.Medico);
            if (cita == null) return NotFound();
            return Ok(cita);
        }

        // NUEVO: Endpoint Historial de Paciente
        [HttpGet("historial/{pacienteId}")]
        [Authorize(Roles = "Admin,Medico,Paciente")]
        public async Task<IActionResult> GetHistorialPaciente(int pacienteId)
        {
            var historial = await _citaRepository.GetHistorialPacienteAsync(pacienteId);
            if (historial == null || !historial.Any())
            {
                return NotFound("No se encontraron citas para este paciente.");
            }
            return Ok(historial);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _citaRepository.Delete(id);
            if (!result) return NotFound();
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Medico")]
        public async Task<IActionResult> Update(int id, CitaDto cita)
        {
            var existingCita = await _citaRepository.GetById(id);
            if (existingCita == null) return NotFound();

            // Nota: Aquí también podrías llamar a las validaciones de horario antes de actualizar.
            existingCita.PacienteId = cita.PacienteId;
            existingCita.MedicoId = cita.MedicoId;
            existingCita.FechaHora = cita.FechaHora;
            existingCita.Motivo = cita.Motivo;

            await _citaRepository.Update(existingCita);
            return Ok(existingCita);
        }
    }
}