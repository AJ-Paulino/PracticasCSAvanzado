using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticasCSAvanzado.Custom;
using InfraestructureLayer.Context;
using DomainLayer.Models;
using DomainLayer.DTO;
using Microsoft.AspNetCore.Authorization;

namespace PracticasCSAvanzado.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly PracticasCSAvanzadoContext _practicasCSAvanzadoContext;
        private readonly Utility _utility;
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
                Password = _utility.EncriptarSHA256(usuario.Password)
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
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = $"Bienvenido {usuarioRegistrado.Nombre}.", token = _utility.GenerarJWT(usuarioRegistrado)});
        }
    }
}
