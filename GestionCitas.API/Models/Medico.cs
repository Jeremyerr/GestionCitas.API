using GestionCitas.API.Models;

public class Medico : BaseModel
{
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NumeroJVPM { get; set; } = string.Empty;
    public int EspecialidadId { get; set; }
    public virtual Especialidad Especialidad { get; set; } = null!;
}