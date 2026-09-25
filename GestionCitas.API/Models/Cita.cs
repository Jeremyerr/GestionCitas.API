namespace GestionCitas.API.Models
{
    public class Cita : BaseModel
    {
        // Relación con Paciente
        public int PacienteId { get; set; }
        public virtual Paciente? Paciente { get; set; }

        // Relación con Medico
        public int MedicoId { get; set; }
        public virtual Medico? Medico { get; set; }

        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}