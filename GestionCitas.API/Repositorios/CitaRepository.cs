using GestionCitas.API.Data;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;

namespace GestionCitas.API.Repositorios
{
    public class CitaRepository : BaseRepository<Cita>, ICitaRepository
    {
        public CitaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}