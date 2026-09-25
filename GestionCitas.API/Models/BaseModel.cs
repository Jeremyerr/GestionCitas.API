namespace GestionCitas.API.Models
{
    public class BaseModel
    {
        public int Id { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        public bool Estado { get; set; } = true;
    }
}