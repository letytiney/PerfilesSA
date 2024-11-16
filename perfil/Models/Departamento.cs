using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace perfil.Models
{
    public class Departamento
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es requerido")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        [Display(Name = "Nombre del Departamento")]
        public string Nombre { get; set; }

        [Display(Name = "Estado")]
        public bool Activo { get; set; }

    }
}