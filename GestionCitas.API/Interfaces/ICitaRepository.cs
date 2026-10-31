using GestionCitas.API.Models;

namespace GestionCitas.API.Interfaces
{
    public interface ICitaRepository : IBaseRepository<Cita>
    {
        // Valida si el médico ya tiene otra cita a esa misma hora
        Task<bool> ExisteCitaConflictivaAsync(int medicoId, DateTime fechaHora);

        // Obtiene todas las citas de un paciente específico
        Task<IEnumerable<Cita>> GetHistorialPacienteAsync(int pacienteId);
    }
}