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
        private readonly PracticasCSAvanzadoContext _PCSA_context;
        private readonly Utility _utility;
        public AccesoController()
        {
            
        }
    }
}
