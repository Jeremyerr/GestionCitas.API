namespace GestionCitas.API.Models
{
    public class Cita : BaseModel
    {
        public int Id { get; set; }

        public string PacienteNombre { get; set; } = string.Empty;

        public string MedicoNombre { get; set; } = string.Empty;

        public DateTime FechaHora { get; set; }

        public string Motivo { get; set; } = string.Empty;
    }
}