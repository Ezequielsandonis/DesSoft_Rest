using System.ComponentModel.DataAnnotations;

namespace Modelos
{
    public class TareaDto
    {
      
            public int TareaId { get; set; }
            [Required(ErrorMessage = "el titulo no puede estar vacio")]
            public string? Titulo { get; set; }
            public DateTime? FechaLimite { get; set; }
            public bool Estado { get; set; }

    }
}
