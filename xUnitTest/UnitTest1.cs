
using ApplicationLayer.Services.TareaServices;
using DomainLayer.DTO;
using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.Commons;
using InfraestructureLayer.Repositorio.TareaRepositorio;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using PracticasCSAvanzado;
using PracticasCSAvanzado.Controllers;
using PracticasCSAvanzado.Hubs;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PracticasCSAvanzado.Custom;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace xUnitTest
{
    public class TareaController_Test
    {

        [Fact]
        public async Task GetTaskAllAsync_DeberiaRetornarListaDeTareas()
        {
            // Arrange
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            var tareas = new List<Tarea> { new Tarea { Id = 1, Description = "Test" } };
            mockCommons.Setup(c => c.GetAllAsync()).ReturnsAsync(tareas);

            var service = new TareaService(mockCommons.Object);

            // Act
            var resultado = await service.GetTaskAllAsync();

            // Assert
            Assert.True(resultado.Succesful);
            Assert.Equal(tareas, resultado.DataList);
        }
        [Fact]
        public async Task GetTaskByIdAllAsync_DeberiaRetornarTareaPorId()
        {
            // Arrange
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            var tarea = new Tarea { Id = 1, Description = "Test" };
            mockCommons.Setup(c => c.GetIdAsync(1)).ReturnsAsync(tarea);

            var service = new TareaService(mockCommons.Object);

            // Act
            var resultado = await service.GetTaskByIdAllAsync(1);

            // Assert
            Assert.True(resultado.Succesful);
            Assert.Equal(tarea, resultado.SingleData);
        }

        [Fact]
        public async Task AddTaskAllAsync_DeberiaAgregarTarea()
        {
            // Arrange
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            var tarea = new Tarea { Id = 1, Description = "Test" };
            mockCommons.Setup(c => c.AddAsync(tarea)).ReturnsAsync((true, "Tarea agregada"));

            var service = new TareaService(mockCommons.Object);

            // Act
            var resultado = await service.AddTaskAllAsync(tarea);

            // Assert
            Assert.True(resultado.Succesful);
            Assert.Equal("Tarea agregada", resultado.Message);
        }

        [Fact]
        public async Task UpdateTaskAllAsync_DeberiaActualizarTarea()
        {
            // Arrange
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            var tarea = new Tarea { Id = 1, Description = "Test" };
            mockCommons.Setup(c => c.UpdateAsync(tarea)).ReturnsAsync((true, "Tarea actualizada"));

            var service = new TareaService(mockCommons.Object);

            // Act
            var resultado = await service.UpdateTaskAllAsync(tarea);

            // Assert
            Assert.True(resultado.Succesful);
            Assert.Equal("Tarea actualizada", resultado.Message);
        }

        [Fact]
        public async Task DeleteTaskAllAsync_DeberiaEliminarTarea()
        {
            // Arrange
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            mockCommons.Setup(c => c.DeleteAsync(1)).ReturnsAsync((true, "Tarea eliminada"));

            var service = new TareaService(mockCommons.Object);

            // Act
            var resultado = await service.DeleteTaskAllAsync(1);

            // Assert
            Assert.True(resultado.Succesful);
            Assert.Equal("Tarea eliminada", resultado.Message);
        }

        [Fact]
        public async Task GetTaskAllAsync_DeberiaFallarSiNoHayTareas()
        {
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            mockCommons.Setup(c => c.GetAllAsync()).ReturnsAsync((IEnumerable<Tarea>)null);

            var service = new TareaService(mockCommons.Object);

            var resultado = await service.GetTaskAllAsync();

            Assert.False(resultado.Succesful);
            Assert.Null(resultado.DataList);
        }

        [Fact]
        public async Task GetTaskByIdAllAsync_DeberiaFallarSiNoExisteTarea()
        {
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            mockCommons.Setup(c => c.GetIdAsync(1)).ReturnsAsync((Tarea)null);

            var service = new TareaService(mockCommons.Object);

            var resultado = await service.GetTaskByIdAllAsync(1);

            Assert.False(resultado.Succesful);
            Assert.Null(resultado.SingleData);
        }

        [Fact]
        public async Task AddTaskAllAsync_DeberiaFallarSiNoSeAgrega()
        {
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            var tarea = new Tarea { Id = 1, Description = "Test" };
            mockCommons.Setup(c => c.AddAsync(tarea)).ReturnsAsync((false, "Error al agregar"));

            var service = new TareaService(mockCommons.Object);

            var resultado = await service.AddTaskAllAsync(tarea);

            Assert.False(resultado.Succesful);
            Assert.Equal("Error al agregar", resultado.Message);
        }

        [Fact]
        public async Task UpdateTaskAllAsync_DeberiaFallarSiNoSeActualiza()
        {
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            var tarea = new Tarea { Id = 1, Description = "Test" };
            mockCommons.Setup(c => c.UpdateAsync(tarea)).ReturnsAsync((false, "Error al actualizar"));

            var service = new TareaService(mockCommons.Object);

            var resultado = await service.UpdateTaskAllAsync(tarea);

            Assert.False(resultado.Succesful);
            Assert.Equal("Error al actualizar", resultado.Message);
        }

        [Fact]
        public async Task DeleteTaskAllAsync_DeberiaFallarSiNoSeElimina()
        {
            var mockCommons = new Mock<ICommonsProcess<Tarea>>();
            mockCommons.Setup(c => c.DeleteAsync(1)).ReturnsAsync((false, "Error al eliminar"));

            var service = new TareaService(mockCommons.Object);

            var resultado = await service.DeleteTaskAllAsync(1);

            Assert.False(resultado.Succesful);
            Assert.Equal("Error al eliminar", resultado.Message);
        }
    }
}
