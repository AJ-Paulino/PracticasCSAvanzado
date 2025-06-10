using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.Commons;

namespace PracticasCSAvanzado
{
    public class Factory : ITareaFactory
    {
        public Tarea TareaAltaPrioridad(string descripcion)
        {
            return new Tarea
            {
                Description = descripcion,
                DueDate = DateTime.Now.AddDays(1),
                Status = "Pendiente",
                AditionalData = "Alta Prioridad"
            };
        }
    }
}
