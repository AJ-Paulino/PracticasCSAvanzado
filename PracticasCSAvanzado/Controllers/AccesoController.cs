using DomainLayer.DTO;
using DomainLayer.Models;
using InfraestructureLayer.Context;
using InfraestructureLayer.Repositorio.Commons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticasCSAvanzado.Custom;

namespace PracticasCSAvanzado.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {

        private readonly Utility _utility;
        private readonly PracticasCSAvanzadoContext _practicasCSAvanzadoContext;
        //private readonly Utility _utility;
        public AccesoController(PracticasCSAvanzadoContext practicasCSAvanzadoContext, Utility utility)
        {
            _practicasCSAvanzadoContext = practicasCSAvanzadoContext;
            _utility = utility;
        }

        [HttpPost]
        [Route("Registrar_Usuario")]
        public async Task<IActionResult> RegistrarUsuario(UsuarioDTO usuario)
        {
            var modeloUsuario = new Usuario
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,
                Password = _utility.EncriptarSHA256(usuario.Password),
                FechaRegistro = usuario.FechaRegistro,
                Rol = usuario.Rol
            };

            await _practicasCSAvanzadoContext.Usuarios.AddAsync(modeloUsuario);
            await _practicasCSAvanzadoContext.SaveChangesAsync();

            if (modeloUsuario.Id != 0)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Usuario registrado correctamente." });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No se pudo registrar el usuario." });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var usuarioRegistrado = await _practicasCSAvanzadoContext.Usuarios.Where(u =>
                u.Email == login.Email &&
                u.Password == _utility.EncriptarSHA256(login.Password)
                ).FirstOrDefaultAsync();

            if (usuarioRegistrado == null)
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Usuario o contraseña incorrecta." });
            else
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = $"Bienvenido {usuarioRegistrado.Nombre}.", token = _utility.GenerarJWT(usuarioRegistrado) });
        }

        [HttpDelete]
        [Route("Eliminar_Usuario/{id}")]
        public async Task<IActionResult> DeleteUserAllAsync(int id)
        {

            var usuario = await _practicasCSAvanzadoContext.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _practicasCSAvanzadoContext.Usuarios.Remove(usuario);
                await _practicasCSAvanzadoContext.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Usuario eliminado." });
            }
            else
            {
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No se pudo eliminar el usuario." });
            }

        }
    }
}
