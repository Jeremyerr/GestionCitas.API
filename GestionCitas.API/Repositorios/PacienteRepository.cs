using GestionCitas.API.Data;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;

namespace GestionCitas.API.Repositorios
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}