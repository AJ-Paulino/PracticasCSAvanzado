﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Tarea
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string AditionalData { get; set; }
    }

    public class TareaRequest
    {
        public string Description { get; set; }
    }

    public interface ITareaFactory
    {
        Tarea TareaAltaPrioridad(string descripcion);
    }
}
