using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCitas.API.Data;
using GestionCitas.API.Models;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        // Conexióna la base de datos
        private readonly AppDbContext _context;

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LEER TODOS (GET)
        [HttpGet]
        public async Task<IActionResult> GetPacientes()
        {
            // Consulta real a la base de datos con Entity Framework
            var pacientes = await _context.Pacientes.Where(p => p.Estado == true).ToListAsync();
            return Ok(pacientes);
        }

        // 2. LEER UNO SOLO POR ID (GET)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound(new { mensaje = "Paciente no encontrado" });
            return Ok(paciente);
        }

        // 3. CREAR O GUARDAR (POST)
        [HttpPost]
        public async Task<IActionResult> PostPaciente([FromBody] Paciente nuevoPaciente)
        {
            nuevoPaciente.FechaRegistro = DateTime.Now;
            nuevoPaciente.Estado = true;

            // Guardado real en la base de datos
            _context.Pacientes.Add(nuevoPaciente);
            await _context.SaveChangesAsync();

            return StatusCode(201, nuevoPaciente);
        }

        // 4. ACTUALIZAR (PUT)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, [FromBody] Paciente pacienteActualizado)
        {
            var pacienteExistente = await _context.Pacientes.FindAsync(id);
            if (pacienteExistente == null) return NotFound(new { mensaje = "Paciente no encontrado" });

            pacienteExistente.Nombres = pacienteActualizado.Nombres;
            pacienteExistente.Apellidos = pacienteActualizado.Apellidos;
            pacienteExistente.Telefono = pacienteActualizado.Telefono;
            pacienteExistente.CorreoElectronico = pacienteActualizado.CorreoElectronico;

            // Actualización real
            _context.Entry(pacienteExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(pacienteExistente);
        }

        // 5. ELIMINAR O BORRADO LÓGICO (DELETE)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null) return NotFound(new { mensaje = "Paciente no encontrado" });

            paciente.Estado = false;
            await _context.SaveChangesAsync(); // Guarda el cambio de estado en la BD

            return Ok(new { mensaje = "Paciente eliminado" });
        }
    }
}
