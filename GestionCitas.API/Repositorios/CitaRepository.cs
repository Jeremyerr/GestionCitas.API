using GestionCitas.API.Data;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionCitas.API.Repositorios
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        private readonly AppDbContext _context;

        public CitaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<bool> ExisteCitaConflictivaAsync(int medicoId, DateTime fechaHora)
        {
            // Retorna true si ya existe una cita para ese médico a esa hora exacta
            return await _context.Set<Cita>()
                .AnyAsync(c => c.MedicoId == medicoId && c.FechaHora == fechaHora);
        }

        public async Task<IEnumerable<Cita>> GetHistorialPacienteAsync(int pacienteId)
        {
            // Trae las citas del paciente ordenadas de la más reciente a la más antigua
            return await _context.Set<Cita>()
                .Include(c => c.Medico)
                    .ThenInclude(m => m.Especialidad)
                .Where(c => c.PacienteId == pacienteId)
                .OrderByDescending(c => c.FechaHora)
                .ToListAsync();
        }
    }
}