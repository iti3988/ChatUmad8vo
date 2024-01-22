using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace estandaresFinal.Data.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Apellido")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(15, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Matrícula")]
        public string? Enrollment { get; set; }

        [JsonIgnore]
        public ICollection<Message>? Messages { get; set; }
    }
}
