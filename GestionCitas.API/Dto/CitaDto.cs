using System.ComponentModel.DataAnnotations;

namespace GestionCitas.API.Dto
{
    public class CitaDto
    {
        [Required(ErrorMessage = "El Paciente es obligatorio.")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "El Médico es obligatorio.")]
        public int MedicoId { get; set; }

        [Required(ErrorMessage = "La fecha y hora de la cita son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "Debe especificar un motivo para la cita.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "El motivo debe tener entre 5 y 200 caracteres.")]
        public string Motivo { get; set; } = string.Empty;
    }
}