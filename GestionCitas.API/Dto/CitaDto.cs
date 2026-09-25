namespace GestionCitas.API.Dto
{
    public class CitaDto
    {
        public int PacienteId { get; set; }
        public int MedicoId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; } = string.Empty;
    }
}