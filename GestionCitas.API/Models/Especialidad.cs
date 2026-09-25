namespace GestionCitas.API.Models
{
    public class Especialidad : BaseModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }
}