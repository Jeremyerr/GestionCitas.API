using GestionCitas.API.Data;
using GestionCitas.API.Models;
using Microsoft.AspNetCore.Identity;

namespace GestionCitas.API.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            // 1. Obtener los servicios necesarios
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<AppDbContext>(); // Agregamos el contexto de BD

            foreach (var role in Roles.Todos)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole { Name = role });
                }
            }

            var superUsuario = "admin@gestioncitas.com";
            if (await userManager.FindByEmailAsync(superUsuario) == null)
            {
                var usuario = new ApplicationUser
                {
                    UserName = "Administrador",
                    Email = superUsuario,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(usuario, "SuperContraseña123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(usuario, Roles.Todos);
                }
            }

            if (!context.Especialidades.Any())
            {
                context.Especialidades.AddRange(
                    new Especialidad { Nombre = "Cardiología", Descripcion = "Especialista en el sistema cardiovascular" },
                    new Especialidad { Nombre = "Pediatría", Descripcion = "Atención médica integral para niños" },
                    new Especialidad { Nombre = "Dermatología", Descripcion = "Especialista en el cuidado de la piel" }
                );
                await context.SaveChangesAsync();
            }


            if (!context.Medicos.Any())
            {

                var cardiologiaId = context.Especialidades.FirstOrDefault(e => e.Nombre == "Cardiología")?.Id ?? 1;
                var pediatriaId = context.Especialidades.FirstOrDefault(e => e.Nombre == "Pediatría")?.Id ?? 2;

                context.Medicos.AddRange(
                    new Medico
                    {
                        Nombres = "Dr. Roberto",
                        Apellidos = "Méndez",
                        NumeroJVPM = "12345",
                        EspecialidadId = cardiologiaId,
                        HoraInicio = new TimeSpan(8, 0, 0),
                        HoraFin = new TimeSpan(16, 0, 0)
                    },
                    new Medico
                    {
                        Nombres = "Dra. Elena",
                        Apellidos = "Castro",
                        NumeroJVPM = "54321",
                        EspecialidadId = pediatriaId,
                        HoraInicio = new TimeSpan(9, 0, 0),
                        HoraFin = new TimeSpan(17, 0, 0)
                    }
                );
                await context.SaveChangesAsync();
            }


            if (!context.Pacientes.Any())
            {

                context.Pacientes.AddRange(
                    new Paciente { Nombres = "Carlos", Apellidos = "Gómez" },
                    new Paciente { Nombres = "Ana", Apellidos = "Martínez" }
                );
                await context.SaveChangesAsync();
            }
        }
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Medico = "Medico";
        public const string Paciente = "Paciente";

        public static List<string> Todos = new List<string> { Admin, Medico, Paciente };
    }
}