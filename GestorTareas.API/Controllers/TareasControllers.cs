using Microsoft.AspNetCore.Mvc;
using Modelos; 
using TareasRepositorio;
using GestorTareas.API.Models.DTO;

namespace GestorTareas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TareaRepositorio _repositorio;

        public TareasController(TareaRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        [HttpGet]
        public IActionResult ListarTareas()
        {
            var tareas = _repositorio.ListarTareas();
            return Ok(tareas); // Devuelve un JSON con el resultado
        }

        [HttpGet("{id}")]
        public IActionResult ObtenerTareaPorId(int id)
        {
            var tarea = _repositorio.ListarTareaPorId(id);
            if (tarea == null)
            {
                return NotFound(); // Devuelve un 404 si no se encuentra la tarea
            }
            return Ok(tarea); // Devuelve un JSON con el resultado
        }

        [HttpPost]
        public IActionResult InsertarTarea(TareaDto tareaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve un 400 si el modelo no es válido
            }

            // Crea una nueva instancia de Tarea utilizando los datos recibidos en el DTO
            var tarea = new TareaDto
            {
                Titulo = tareaDto.Titulo,
                FechaLimite = tareaDto.FechaLimite,
                Estado = tareaDto.Estado
            };

            _repositorio.InsertarTarea(tarea);
            return CreatedAtAction(nameof(ObtenerTareaPorId), new { id = tarea.TareaId }, tarea); // Devuelve un 201 con la tarea creada
        }

        [HttpPut("{id}")]
        public IActionResult ActualizarTarea(int id, TareaDto tareaDto)
        {
            var tareaExistente = _repositorio.ListarTareaPorId(id);
            if (tareaExistente == null)
            {
                return NotFound(); // Devuelve un 404 si no se encuentra la tarea
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devuelve un 400 si el modelo no es válido
            }

            tareaExistente.Titulo = tareaDto.Titulo;
            tareaExistente.FechaLimite = tareaDto.FechaLimite;
            tareaExistente.Estado = tareaDto.Estado;

            _repositorio.ActualizarTarea(tareaExistente);
            return NoContent(); // Devuelve un 204 si la tarea se actualizó correctamente
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarTarea(int id)
        {
            var tareaExistente = _repositorio.ListarTareaPorId(id);
            if (tareaExistente == null)
            {
                return NotFound(); // Devuelve un 404 si no se encuentra la tarea
            }

            _repositorio.EliminarTarea(id);
            return NoContent(); // Devuelve un 204 si la tarea se eliminó correctamente
        }
    }
}
