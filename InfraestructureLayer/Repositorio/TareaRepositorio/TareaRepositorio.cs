using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.Commons;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraestructureLayer.Repositorio.TareaRepositorio
{
    public class TareaRepositorio : ICommonsProcess<Tarea>
    {
        private readonly PracticasCSAvanzadoContext _practicasCSAvanzadoContext;
        public TareaRepositorio(PracticasCSAvanzadoContext practicasCSAvanzadoContext)
        {
            _practicasCSAvanzadoContext = practicasCSAvanzadoContext;
        }
        public async Task<IEnumerable<Tarea>> GetAllAsync()
        => await _practicasCSAvanzadoContext.Tareas.ToListAsync();

        public async Task<Tarea> GetIdAsync(int id)
        => await _practicasCSAvanzadoContext.Tareas.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<(bool IsSuccess, string Message)> AddAsync(Tarea entry)
        {
            try
            {
                //Validación de tarea duplicada
                var exists = _practicasCSAvanzadoContext.Tareas.Any(x => x.Description == entry.Description);

                if (exists)
                {
                    return (false, "Ya existe una tarea con esa descripción.");
                }

                await _practicasCSAvanzadoContext.Tareas.AddAsync(entry);
                await _practicasCSAvanzadoContext.SaveChangesAsync();
                return (true, "Tarea guardada correctamente.");
            }
            catch (Exception e)
            {
                return (false, $"No se pudo guardar la tarea. {e.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> UpdateAsync(Tarea entry)
        {
            try
            {
                //Validación de información igual
                var exists = _practicasCSAvanzadoContext.Tareas.Any(x => x.Description == entry.Description);

                if (exists)
                {
                    return (false, "La información que desea actualizar es la misma a la existente.");
                }
                _practicasCSAvanzadoContext.Tareas.Update(entry);
                await _practicasCSAvanzadoContext.SaveChangesAsync();
                return (true, "Tarea actualizada correctamente.");
            }
            catch (Exception e)
            {
                return (false, $"No se pudo actualizar la tarea. {e.Message}");
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {
            try
            {
                var tarea = await _practicasCSAvanzadoContext.Tareas.FindAsync(id);
                if ( tarea != null)
                {
                    _practicasCSAvanzadoContext.Tareas.Remove(tarea);
                    await _practicasCSAvanzadoContext.SaveChangesAsync();
                    return (true, "Tarea eliminada correctamente.");
                }
                else
                {
                    return (false, "No se encontró la tarea a eliminar.");
                }
            }
            catch (Exception e)
            {
                return (false, $"No se pudo eliminar la tarea. {e.Message}");
            }
        }                
    }
}
