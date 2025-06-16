using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraestructureLayer.Context
{
    public class PracticasCSAvanzadoContext : DbContext
    {
        public PracticasCSAvanzadoContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Tarea> Tareas { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }
    }
}
