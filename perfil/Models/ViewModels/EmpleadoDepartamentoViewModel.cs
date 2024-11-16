using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace perfil.Models.ViewModels
{
    public class EmpleadoDepartamentoViewModel
    {
        public string NombreDepartamento { get; set; }
        public bool DepartamentoActivo { get; set; }
        public int TotalEmpleados { get; set; }
        public List<EmpleadoViewModel> Empleados { get; set; }
    }
    public class EmpleadoViewModel
    {
        public string Nombres { get; set; }
        public string DPI { get; set; }
        public string NIT { get; set; }
        public int Edad { get; set; }
        public string TiempoLaborando { get; set; }
        public DateTime FechaIngreso { get; set; }
    }
}