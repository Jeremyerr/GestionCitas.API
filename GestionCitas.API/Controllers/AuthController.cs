using GestionCitas.API.Dto;
using GestionCitas.API.Interfaces;
using GestionCitas.API.Models;
using GestionCitas.API.Services;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMedicoRepository _medicoRepository; // INYECTADO PARA CREAR EL PERFIL

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            TokenServices tokenServices,
            IMedicoRepository medicoRepository) // SE AGREGA AL CONSTRUCTOR
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenServices = tokenServices;
            _medicoRepository = medicoRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

            var resultado = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!resultado) return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenServices.GenerarToken(user, roles.ToList());

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var applicationUser = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var resultado = await _userManager.CreateAsync(applicationUser, dto.Password);
            if (!resultado.Succeeded) return BadRequest(resultado.Errors);

            await _userManager.AddToRoleAsync(applicationUser, "Paciente");
            return Ok(new { mensaje = "Usuario creado correctamente" });
        }

        [HttpPost("register-medico")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterMedico(RegisterMedicoDto dto) // USAMOS EL NUEVO DTO
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // 1. Crear el usuario para el Login
            var applicationUser = new ApplicationUser
            {
                UserName = dto.Username,
                Email = dto.Email
            };

            var resultado = await _userManager.CreateAsync(applicationUser, dto.Password);

            // Prevención Error 3: Si el correo ya existe o la clave es débil, se detiene aquí.
            if (!resultado.Succeeded) return BadRequest(resultado.Errors);

            await _userManager.AddToRoleAsync(applicationUser, "Medico");

            // 2. Crear el perfil en la base de datos (Prevención Error 1 y 2)
            try
            {
                await _medicoRepository.CreateAsync(new Medico
                {
                    Nombres = dto.Nombres,
                    Apellidos = dto.Apellidos,
                    NumeroJVPM = dto.NumeroJVPM,
                    EspecialidadId = dto.EspecialidadId
                    // Las horas de inicio y fin tomarán tus valores por defecto automáticamente
                });
            }
            catch (Exception ex)
            {
                // Prevención de datos corruptos: Si falla la creación del perfil, borramos el usuario que acabamos de crear
                await _userManager.DeleteAsync(applicationUser);
                return StatusCode(500, new { mensaje = "Error al crear el perfil del médico. Se canceló la creación de la cuenta.", detalle = ex.Message });
            }

            return Ok(new { mensaje = "Cuenta de acceso y perfil de Médico creados correctamente" });
        }
    }
}