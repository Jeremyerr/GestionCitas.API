using Microsoft.EntityFrameworkCore;
using GestionCitas.API.Models;

namespace GestionCitas.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}