using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using GestorTareas.Web.Models;


namespace GestorTareas.Web.Controllers
{
    public class TareasController : Controller
    {
        private readonly HttpClient _httpClient;

        public TareasController(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:7045/api/Tareas/");
        }

        // GET: Tareas
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error al obtener la lista de tareas.");
            }

            var tareas = await response.Content.ReadFromJsonAsync<List<TareasViewModel>>();
            return View(tareas);
        }

        // GET: Tareas/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Tarea no encontrada.");
            }
            var tarea = await response.Content.ReadFromJsonAsync<TareasViewModel>();
            return View(tarea);
        }

        // GET: Tareas/Create
        public IActionResult Create()
        {
            var tarea = new TareasViewModel(); 
            return View(tarea);
        }

      
        // POST: Tareas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TareasViewModel tarea)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("", tarea);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error al crear la tarea.");
                }
                return RedirectToAction(nameof(Index));
            }
            // Si ModelState no es válido, maneja el error y retorna la vista con el modelo
            return View(tarea);
        }


        // GET: Tareas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Tarea no encontrada.");
            }
            var tarea = await response.Content.ReadFromJsonAsync<TareasViewModel>();
            return View(tarea);
        }

        // POST: Tareas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TareasViewModel tarea)
        {
            if (id != tarea.TareaId)
            {
                return BadRequest("ID de tarea no coincide.");
            }

            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"{id}", tarea);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error al actualizar la tarea.");
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tarea);
        }

        // GET: Tareas/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.GetAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound("Tarea no encontrada.");
            }
            var tarea = await response.Content.ReadFromJsonAsync<TareasViewModel>();
            return View(tarea);
        }

        // POST: Tareas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _httpClient.DeleteAsync($"{id}");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error al eliminar la tarea.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
