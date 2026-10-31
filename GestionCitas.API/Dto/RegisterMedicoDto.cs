using System.ComponentModel.DataAnnotations;

namespace GestionCitas.API.Dto
{
    public class RegisterMedicoDto : RegisterDto
    {
        [Required(ErrorMessage = "Los nombres son obligatorios.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombres { get; set; } = string.Empty;

        [Required(ErrorMessage = "Los apellidos son obligatorios.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El apellido debe tener entre 3 y 50 caracteres.")]
        public string Apellidos { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de JVPM es obligatorio.")]
        [RegularExpression(@"^[0-9]{4,6}$", ErrorMessage = "El número JVPM debe tener un formato válido de 4 a 6 dígitos.")]
        public string NumeroJVPM { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe asignar una especialidad.")]
        public int EspecialidadId { get; set; }
    }
}