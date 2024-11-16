using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using perfil.Models;
using System.Collections.Generic;

namespace perfil.Controllers
{
    public class EmpleadoController : Controller
    {
         private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Index()
        {
            var empleados = new List<Empleado>();
            var departamentos = new List<Departamento>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string depQuery = "SELECT Id, Nombre FROM Departamentos WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(depQuery, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamentos.Add(new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }

                string empQuery = @"SELECT e.*, d.Nombre as DepartamentoNombre 
                              FROM Empleados e 
                              LEFT JOIN Departamentos d ON e.DepartamentoId = d.Id";
                using (SqlCommand cmd = new SqlCommand(empQuery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empleados.Add(new Empleado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombres = reader["Nombres"].ToString(),
                                DPI = reader["DPI"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                Genero = reader["Genero"].ToString(),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                                Direccion = reader["Direccion"].ToString(),
                                NIT = reader["NIT"].ToString(),
                                DepartamentoId = reader["DepartamentoId"] != DBNull.Value ?
                                               Convert.ToInt32(reader["DepartamentoId"]) : (int?)null,
                                DepartamentoNombre = reader["DepartamentoNombre"]?.ToString()
                            });
                        }
                    }
                }
            }

            ViewBag.Departamentos = departamentos;
            return View(empleados);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsignarDepartamento(int empleadoId, int? departamentoId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Empleados SET DepartamentoId = @DepartamentoId WHERE Id = @EmpleadoId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpleadoId", empleadoId);
                    cmd.Parameters.AddWithValue("@DepartamentoId", (object)departamentoId ?? DBNull.Value);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Empleado empleado = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT e.*, 
                        d.Nombre AS DepartamentoNombre,
                        d.Activo AS DepartamentoActivo
                        FROM Empleados e
                        LEFT JOIN Departamentos d ON e.DepartamentoId = d.Id
                        WHERE e.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            empleado = new Empleado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombres = reader["Nombres"].ToString(),
                                DPI = reader["DPI"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                Genero = reader["Genero"].ToString(),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                                Direccion = reader["Direccion"].ToString(),
                                NIT = reader["NIT"].ToString(),
                                DepartamentoId = reader["DepartamentoId"] != DBNull.Value ?
                                               Convert.ToInt32(reader["DepartamentoId"]) : (int?)null,
                                DepartamentoNombre = reader["DepartamentoNombre"]?.ToString(),
                                DepartamentoActivo = reader["DepartamentoActivo"] != DBNull.Value ?
                                                   Convert.ToBoolean(reader["DepartamentoActivo"]) : (bool?)null
                            };
                        }
                    }
                }
            }

            if (empleado == null)
            {
                return HttpNotFound();
            }

            return View(empleado);
        }
        public ActionResult Create()
        {
            var departamentos = new List<Departamento>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre FROM Departamentos WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamentos.Add(new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
            }

            ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre");
            return View(new Empleado { FechaIngreso = DateTime.Now }); // Fecha de ingreso por defecto
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            cmd.Parameters.AddWithValue("@Nombres", empleado.Nombres);
                            cmd.Parameters.AddWithValue("@DPI", empleado.DPI);
                            cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                            cmd.Parameters.AddWithValue("@Genero", empleado.Genero);
                            cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                            cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion);
                            cmd.Parameters.AddWithValue("@NIT", empleado.NIT);
                            cmd.Parameters.AddWithValue("@DepartamentoId",
                                (object)empleado.DepartamentoId ?? DBNull.Value);

                            conn.Open();
                            var result = cmd.ExecuteScalar(); 

                            if (result != null)
                            {
                                empleado.Id = Convert.ToInt32(result);
                                TempData["Success"] = "Empleado creado exitosamente.";
                                return RedirectToAction("Index");
                            }
                        }
                        catch (SqlException ex)
                        {
                            switch (ex.Message)
                            {
                                case string msg when msg.Contains("fecha de ingreso debe ser posterior"):
                                    ModelState.AddModelError("FechaIngreso",
                                        "La fecha de ingreso debe ser posterior a la fecha de nacimiento.");
                                    break;

                                case string msg when msg.Contains("18 años"):
                                    ModelState.AddModelError("FechaIngreso",
                                        "El empleado debe tener al menos 18 años al momento de ingresar.");
                                    break;

                                case string msg when msg.Contains("DPI"):
                                    ModelState.AddModelError("DPI",
                                        "Ya existe un empleado registrado con este DPI.");
                                    break;

                                case string msg when msg.Contains("NIT"):
                                    ModelState.AddModelError("NIT",
                                        "Ya existe un empleado registrado con este NIT.");
                                    break;

                                case string msg when msg.Contains("departamento"):
                                    ModelState.AddModelError("DepartamentoId",
                                        "El departamento seleccionado no existe o no está activo.");
                                    break;

                                case string msg when msg.Contains("campos obligatorios"):
                                    ModelState.AddModelError("",
                                        "Todos los campos obligatorios deben ser proporcionados.");
                                    break;

                                default:
                                    ModelState.AddModelError("",
                                        "Error al guardar el empleado: " + ex.Message);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("",
                                "Error inesperado al guardar el empleado: " + ex.Message);
                        }
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var departamentos = new List<Departamento>();
                string query = "SELECT Id, Nombre FROM Departamentos WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamentos.Add(new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
                ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre",
                    empleado.DepartamentoId);
            }

            // Devolver la vista con los errores
            return View(empleado);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarEmpleado", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            // Configurar los parámetros
                            cmd.Parameters.AddWithValue("@Id", empleado.Id);
                            cmd.Parameters.AddWithValue("@Nombres", empleado.Nombres);
                            cmd.Parameters.AddWithValue("@DPI", empleado.DPI);
                            cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                            cmd.Parameters.AddWithValue("@Genero", empleado.Genero);
                            cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                            cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion);
                            cmd.Parameters.AddWithValue("@NIT", empleado.NIT);
                            cmd.Parameters.AddWithValue("@DepartamentoId",
                                (object)empleado.DepartamentoId ?? DBNull.Value);

                            conn.Open();
                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                TempData["Success"] = "Empleado actualizado exitosamente.";
                                return RedirectToAction("Index");
                            }
                        }
                        catch (SqlException ex)
                        {
                            switch (ex.Message)
                            {
                                case string msg when msg.Contains("no existe"):
                                    ModelState.AddModelError("",
                                        "El empleado que intenta actualizar no existe.");
                                    break;

                                case string msg when msg.Contains("fecha de ingreso debe ser posterior"):
                                    ModelState.AddModelError("FechaIngreso",
                                        "La fecha de ingreso debe ser posterior a la fecha de nacimiento.");
                                    break;

                                case string msg when msg.Contains("DPI"):
                                    ModelState.AddModelError("DPI",
                                        "Ya existe otro empleado registrado con este DPI.");
                                    break;

                                case string msg when msg.Contains("NIT"):
                                    ModelState.AddModelError("NIT",
                                        "Ya existe otro empleado registrado con este NIT.");
                                    break;

                                case string msg when msg.Contains("departamento"):
                                    ModelState.AddModelError("DepartamentoId",
                                        "El departamento seleccionado no existe o no está activo.");
                                    break;

                                case string msg when msg.Contains("campos obligatorios"):
                                    ModelState.AddModelError("",
                                        "Todos los campos obligatorios deben ser proporcionados.");
                                    break;

                                default:
                                    ModelState.AddModelError("",
                                        "Error al actualizar el empleado: " + ex.Message);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("",
                                "Error inesperado al actualizar el empleado: " + ex.Message);
                        }
                    }
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                var departamentos = new List<Departamento>();
                string query = "SELECT Id, Nombre FROM Departamentos WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamentos.Add(new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
                ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre",
                    empleado.DepartamentoId);
            }

            return View(empleado);
        }

        public ActionResult Edit(int id)
        {
            Empleado empleado = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT e.*, d.Nombre as DepartamentoNombre 
                        FROM Empleados e 
                        LEFT JOIN Departamentos d ON e.DepartamentoId = d.Id 
                        WHERE e.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            empleado = new Empleado
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombres = reader["Nombres"].ToString(),
                                DPI = reader["DPI"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                Genero = reader["Genero"].ToString(),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                                Direccion = reader["Direccion"].ToString(),
                                NIT = reader["NIT"].ToString(),
                                DepartamentoId = reader["DepartamentoId"] != DBNull.Value ?
                                    Convert.ToInt32(reader["DepartamentoId"]) : (int?)null,
                                DepartamentoNombre = reader["DepartamentoNombre"]?.ToString()
                            };
                        }
                    }
                }
                var departamentos = new List<Departamento>();
                string depQuery = "SELECT Id, Nombre FROM Departamentos WHERE Activo = 1";
                using (SqlCommand cmd = new SqlCommand(depQuery, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamentos.Add(new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString()
                            });
                        }
                    }
                }
                ViewBag.Departamentos = new SelectList(departamentos, "Id", "Nombre",
                    empleado?.DepartamentoId);
            }

            if (empleado == null)
            {
                return HttpNotFound();
            }

            return View(empleado);
        }
    }
}