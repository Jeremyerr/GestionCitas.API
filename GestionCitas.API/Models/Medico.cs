using GestionCitas.API.Models;

public class Medico : BaseModel
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NumeroJVPM { get; set; } = string.Empty;
    public int EspecialidadId { get; set; }
    public virtual Especialidad Especialidad { get; set; } = null!;
    public TimeSpan HoraInicio { get; set; } = new TimeSpan(8, 0, 0); // Default: 08:00 AM
    public TimeSpan HoraFin { get; set; } = new TimeSpan(16, 0, 0); // Default: 04:00 PM
}