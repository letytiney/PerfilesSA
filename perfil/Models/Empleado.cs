using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace perfil.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombres { get; set; }
        public string DPI { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Genero { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Direccion { get; set; }
        public string NIT { get; set; }
        public int? DepartamentoId { get; set; }
        public int Edad
        {
            get
            {
                return DateTime.Today.Year - FechaNacimiento.Year -
                    (DateTime.Today.DayOfYear < FechaNacimiento.DayOfYear ? 1 : 0);
            }
        }

        public string TiempoLaborando
        {
            get
            {
                var tiempo = DateTime.Today - FechaIngreso;
                var años = tiempo.Days / 365;
                var meses = (tiempo.Days % 365) / 30;
                return $"{años} años, {meses} meses";
            }
        }
        public virtual Departamento Departamento { get; set; }

        public string DepartamentoNombre { get; set; }

        public bool? DepartamentoActivo { get; set; }
    }
}