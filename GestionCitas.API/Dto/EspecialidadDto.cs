using System.ComponentModel.DataAnnotations;

namespace GestionCitas.API.Dto
{
    public class EspecialidadDto
    {
        [Required(ErrorMessage = "El nombre de la especialidad es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(250, MinimumLength = 10, ErrorMessage = "La descripción debe tener entre 10 y 250 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;
    }
}