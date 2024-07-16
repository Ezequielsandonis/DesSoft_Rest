namespace GestorTareas.Web.Models
{
    public class TareasViewModel
    {
        public int TareaId { get; set; }
        public string? Titulo { get; set; }
        public DateTime? FechaLimite { get; set; }
        public bool Estado { get; set; }
    }
}
