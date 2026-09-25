using GestionCitas.API.Data;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;

namespace GestionCitas.API.Repositorios
{
    public class MedicoRepository : BaseRepository<Medico>, IMedicoRepository
    {
        public MedicoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}