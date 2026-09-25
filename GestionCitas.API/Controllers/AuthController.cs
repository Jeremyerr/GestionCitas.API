using GestionCitas.API.Dto;
using GestionCitas.API.Models;
using GestionCitas.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GestionCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly TokenServices _tokenServices;

        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, TokenServices tokenServices)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenServices = tokenServices;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
            }

            var resultado = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!resultado)
            {
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenServices.GenerarToken(user, roles.ToList());

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var applicationUser = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var resultado = await _userManager.CreateAsync(applicationUser, dto.Password);
            if (!resultado.Succeeded)
            {
                return BadRequest(resultado.Errors);
            }

            // Opcional: Asignar rol por defecto al registrarse
            await _userManager.AddToRoleAsync(applicationUser, "Paciente");

            return Ok(new { mensaje = "Usuario creado correctamente" });
        }
    }
}