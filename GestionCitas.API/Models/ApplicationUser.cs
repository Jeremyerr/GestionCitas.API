using Microsoft.AspNetCore.Identity;

namespace GestionCitas.API.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Citas = new HashSet<Cita>();
        }

        // Relación opcional si deseas asociar los usuarios con las citas
        public virtual ICollection<Cita> Citas { get; set; }
    }
}