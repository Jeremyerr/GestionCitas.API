using GestionCitas.API.Models;
using Microsoft.AspNetCore.Identity;

namespace GestionCitas.API.Data.Seed
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

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