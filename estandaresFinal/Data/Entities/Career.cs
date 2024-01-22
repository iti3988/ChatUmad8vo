using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace estandaresFinal.Data.Entities
{
    public class Career : IEntity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Nombre")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(25, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Acrónimo")]
        public string? Acronym { get; set; }

        [JsonIgnore]
        public ICollection<Student>? Students { get; set; } = new List<Student>();

    }
}
