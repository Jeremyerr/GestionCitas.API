using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize] -> Lo activaremos cuando implementemos JWT
    public class CitasController : ControllerBase // Luego cambiaremos a CustomBaseController
    {
        private readonly ICitaRepository _citaRepository;

        public CitasController(ICitaRepository citaRepository)
        {
            _citaRepository = citaRepository;
        }

        [HttpPost]
        // [Authorize(Roles = "Admin, Usuario")]
        public async Task<IActionResult> Create(CitaDto cita)
        {
            // var usuarioLogueado = GetUserId(); -> Pendiente para JWT

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
        public async Task<IActionResult> Get(int skip, int take)
        {
            var citas = await _citaRepository.GetAsync(skip, take);
            return Ok(citas);
        }

        [HttpGet("getById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Aquí usamos la magia de tu IBaseRepository para traer las relaciones
            var cita = await _citaRepository.GetById(id, c => c.Paciente, c => c.Medico);

            if (cita == null)
            {
                return NotFound();
            }

            return Ok(cita);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Esto usa tu borrado físico configurado en BaseRepository
            var result = await _citaRepository.Delete(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CitaDto cita)
        {
            var existingCita = await _citaRepository.GetById(id);
            if (existingCita == null)
            {
                return NotFound();
            }

            // Actualizar las propiedades de la entidad
            existingCita.PacienteId = cita.PacienteId;
            existingCita.MedicoId = cita.MedicoId;
            existingCita.FechaHora = cita.FechaHora;
            existingCita.Motivo = cita.Motivo;

            await _citaRepository.Update(existingCita);
            return Ok(existingCita);
        }
    }
}