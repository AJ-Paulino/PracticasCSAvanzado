using ApplicationLayer.Services.TareaServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.TareaRepositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PracticasCSAvanzado.Custom;
using PracticasCSAvanzado.Hubs;
using System.Threading;

namespace PracticasCSAvanzado.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly TareaService _service;
        private readonly ManejadorTareasSecuencial _manejadorTareas;
        private readonly ITareaFactory _factory;
        private readonly PracticasCSAvanzadoContext _practicasCSAvanzadoContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public TareaController(TareaService service, ManejadorTareasSecuencial manejadorTareas, ITareaFactory factory,
            PracticasCSAvanzadoContext practicasCSAvanzadoContext, IHubContext<NotificationHub> hubContext)
        {
            _service = service;
            _manejadorTareas = manejadorTareas;
            _factory = factory;
            _practicasCSAvanzadoContext = practicasCSAvanzadoContext;
            _hubContext = hubContext;
        }


        [HttpGet]
        [Route("Obtener")]
        public async Task<ActionResult<Response<Tarea>>> GetTaskAllAsync()
            => await _service.GetTaskAllAsync();

        [HttpGet]
        [Route("Obtener/{id}")]
        public async Task<ActionResult<Response<Tarea>>> GetTaskByIdAllAsync(int id)
            => await _service.GetTaskByIdAllAsync(id);

        [HttpPost]
        [Route("Crear")]
        public async Task<ActionResult<Response<string>>> AddTaskAllAsync(Tarea tarea)
        {
            await _service.AddTaskAllAsync(tarea);

            await _hubContext.Clients.All.SendAsync("recibirNotificacion", tarea);
            return Ok("Notificación de creación de tarea enviada a todos los clientes conectados.");
        }

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

                await _hubContext.Clients.All.SendAsync("recibirNotificacion", request);
                Ok("Notificación de creación de tarea de alta prioridad enviada a todos los clientes conectados.");
            });
            return Accepted("Tarea en cola. Se notificará cuando esté completa.");
        }

        [HttpPut]
        [Route("Actualizar")]
        public async Task<ActionResult<Response<string>>> UpdateTaskAllAsync(Tarea tarea)
            => await _service.UpdateTaskAllAsync(tarea);

        [HttpDelete]
        [Route("Eliminar/{id}")]
        public async Task<ActionResult<Response<string>>> DeleteTaskAllAsync(int id)
            => await _service.DeleteTaskAllAsync(id);

        [HttpPost]
        [Route("EnviarNotificacion")]
        public async Task<IActionResult> EnviarNotificacion(string mensaje)
        {
            await _hubContext.Clients.All.SendAsync("recibirNotificacion", mensaje);
            return Ok("Notificación de creación de tarea enviada a todos los clientes conectados.");
        }
    }
}
