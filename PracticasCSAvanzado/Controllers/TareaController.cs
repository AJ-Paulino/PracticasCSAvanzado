using ApplicationLayer.Services.TareaServices;
using DomainLayer.Models;
using DomainLayer.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.TareaRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PracticasCSAvanzado.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace PracticasCSAvanzado.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly TareaService _service;
        private readonly ManejadorTareasSecuencial _manejadorTareas;
        private readonly ITareaFactory _factory;
        private readonly PracticasCSAvanzadoContext _practicasCSAvanzadoContext;

        public TareaController(TareaService service, ManejadorTareasSecuencial manejadorTareas, ITareaFactory factory, PracticasCSAvanzadoContext practicasCSAvanzadoContext)
        {
            _service = service;
            _manejadorTareas = manejadorTareas;
            _factory = factory;
            _practicasCSAvanzadoContext = practicasCSAvanzadoContext;
        }


        [HttpGet]
        [Route("Obtener")]
        public async Task<ActionResult<Response<Tarea>>> GetTaskAllAsync()
            => await _service.GetTaskAllAsync();

        [HttpGet("{id}")]
        [Route("Obtener/{id}")]
        public async Task<ActionResult<Response<Tarea>>> GetTaskByIdAllAsync(int id)
            => await _service.GetTaskByIdAllAsync(id);

        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult<Response<string>>> AddTaskAllAsync(Tarea tarea)
            => await _service.AddTaskAllAsync(tarea);

        [HttpPost]
        [Route("CrearTareaAltaPrioridad")]
        public IActionResult CrearTarea([FromBody] TareaRequest request)
        {
            _manejadorTareas.AgregarTarea(async () =>
            {
                var tarea = _factory.TareaAltaPrioridad(request.Description);
                Console.WriteLine($"Procesando: {tarea.Description}");
                await Task.Delay(1000);
                Console.WriteLine($"Tarea completada: {tarea.Description}");
            });
            return Accepted("Tarea en cola. Se notificará cuando esté completa.");
        }              

        [HttpPut]
        [Route("Actualizar")]
        public async Task<ActionResult<Response<string>>> UpdateTaskAllAsync(Tarea tarea)
            => await _service.UpdateTaskAllAsync(tarea);

        [HttpDelete("{id}")]
        [Route("Eliminar/{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAllAsync(int id)
            => await _service.DeleteTaskAllAsync(id);
    }
}
