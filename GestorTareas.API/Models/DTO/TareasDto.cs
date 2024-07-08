namespace GestorTareas.API.Models.DTO
{
     public class TareasDto
        {
            public int TareaId { get; set; }
            public string? Titulo { get; set; }
            public DateTime? FechaLimite { get; set; }
            public bool Estado { get; set; }
        }
    
}
