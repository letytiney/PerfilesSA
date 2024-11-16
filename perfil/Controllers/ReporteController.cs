using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using perfil.Models.ViewModels;

namespace perfil.Controllers
{
    public class ReporteController : Controller
    {
        private readonly string connectionString;

        public ReporteController()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public ActionResult EmpleadosPorDepartamento()
        {
            var reporteList = new List<EmpleadoDepartamentoViewModel>();

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT 
                        d.Id AS DepartamentoId,
                        d.Nombre AS DepartamentoNombre,
                        d.Activo AS DepartamentoActivo,
                        e.Id AS EmpleadoId,
                        e.Nombres,
                        e.DPI,
                        e.NIT,
                        e.FechaNacimiento,
                        e.FechaIngreso
                    FROM 
                        Departamentos d
                        LEFT JOIN Empleados e ON d.Id = e.DepartamentoId
                    ORDER BY 
                        d.Nombre, e.Nombres";

                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        EmpleadoDepartamentoViewModel currentDepartamento = null;

                        while (reader.Read())
                        {
                            var departamentoNombre = reader.GetString(reader.GetOrdinal("DepartamentoNombre"));

                            if (currentDepartamento == null || currentDepartamento.NombreDepartamento != departamentoNombre)
                            {
                                currentDepartamento = new EmpleadoDepartamentoViewModel
                                {
                                    NombreDepartamento = departamentoNombre,
                                    DepartamentoActivo = reader.GetBoolean(reader.GetOrdinal("DepartamentoActivo")),
                                    Empleados = new List<EmpleadoViewModel>()
                                };
                                reporteList.Add(currentDepartamento);
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("EmpleadoId")))
                            {
                                var fechaNacimiento = reader.GetDateTime(reader.GetOrdinal("FechaNacimiento"));
                                var fechaIngreso = reader.GetDateTime(reader.GetOrdinal("FechaIngreso"));

                                currentDepartamento.Empleados.Add(new EmpleadoViewModel
                                {
                                    Nombres = reader.GetString(reader.GetOrdinal("Nombres")),
                                    DPI = reader.GetString(reader.GetOrdinal("DPI")),
                                    NIT = reader.IsDBNull(reader.GetOrdinal("NIT")) ? "" : reader.GetString(reader.GetOrdinal("NIT")),
                                    Edad = CalcularEdad(fechaNacimiento),
                                    TiempoLaborando = CalcularTiempoLaborando(fechaIngreso),
                                    FechaIngreso = fechaIngreso 
                                });
                            }
                        }

                        foreach (var dept in reporteList)
                        {
                            dept.TotalEmpleados = dept.Empleados.Count;
                        }
                    }
                }
            }

            return View(reporteList);
        }

        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var today = DateTime.Today;
            var edad = today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > today.AddYears(-edad))
                edad--;
            return edad;
        }

        private string CalcularTiempoLaborando(DateTime fechaIngreso)
        {
            var tiempo = DateTime.Today - fechaIngreso;
            var años = tiempo.Days / 365;
            var meses = (tiempo.Days % 365) / 30;
            return $"{años} años, {meses} meses";
        }
    }
}