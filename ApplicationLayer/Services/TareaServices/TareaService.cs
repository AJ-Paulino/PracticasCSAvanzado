using DomainLayer.DTO;
using DomainLayer.Models;
using InfraestructureLayer.Repositorio.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationLayer.Services.TareaServices
{
    public class TareaService
    {
        private readonly ICommonsProcess<Tarea> _commonsProcess;

        public TareaService(ICommonsProcess<Tarea> commonsProcess)
        {
            _commonsProcess = commonsProcess;
        }

        public async Task<Response<Tarea>> GetTaskAllAsync()
        {
            var response = new Response<Tarea>();
            try
            {
                response.DataList = await _commonsProcess.GetAllAsync();
                response.Succesful = true;
            }
            catch (Exception e)
            {
                response.Errors.Add($"Error al obtener las tareas: {e.Message}");
            }
            return response;
        }

        public async Task<Response<Tarea>> GetTaskByIdAllAsync(int id)
        {
            var response = new Response<Tarea>();
            try
            {
                var result = await _commonsProcess.GetIdAsync(id);
                if(result != null)
                {
                    response.SingleData = result;
                    response.Succesful = true;
                }
                else
                {
                    response.Succesful = false;
                    response.Message = "No se encontró la tarea.";
                }
            }
            catch (Exception e)
            {
                response.Errors.Add($"Error al obtener las tareas: {e.Message}");
            }
            return response;
        }

        public async Task<Response<string>> AddTaskAllAsync(Tarea tarea)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonsProcess.AddAsync(tarea);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;
            }
            catch (Exception e)
            {
                response.Errors.Add($"Error al agregar las tareas: {e.Message}");
            }
            return response;
        }

        public async Task<Response<string>> UpdateTaskAllAsync(Tarea tarea)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonsProcess.UpdateAsync(tarea);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;
            }
            catch (Exception e)
            {
                response.Errors.Add($"Error al actualizar las tareas: {e.Message}");
            }
            return response;
        }

        public async Task<Response<string>> DeleteTaskAllAsync(int id)
        {
            var response = new Response<string>();
            try
            {
                var result = await _commonsProcess.DeleteAsync(id);
                response.Message = result.Message;
                response.Succesful = result.IsSuccess;
            }
            catch (Exception e)
            {
                response.Errors.Add($"Error al eliminar las tareas: {e.Message}");
            }
            return response;
        }
    }
}
