using System.ComponentModel.DataAnnotations;

namespace estandaresFinal.Data.Entities
{
    public class Message : IEntity
    {
        public int Id { get; set; }
        public Chat? Chat { get; set; }
        public User? User { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha publicación")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "{0} es obligatorio.")]
        [MaxLength(250, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        [Display(Name = "Mensaje")]
        public string? MessageDescription { get; set; }

        [Display(Name = "Archivo")]
        public string? File { get; set; }
    }
}
