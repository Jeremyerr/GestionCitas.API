using GestionCitas.API.Data;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;

namespace GestionCitas.API.Repositorios
{
    public class EspecialidadRepository : BaseRepository<Especialidad>, IEspecialidadRepository
    {
        public EspecialidadRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}