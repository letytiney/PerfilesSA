using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using perfil.Models;
using System.Collections.Generic;

namespace perfil.Controllers
{
    public class DepartamentoController : Controller
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult Index()
        {
            List<Departamento> departamentos = new List<Departamento>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Departamentos", conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departamentos.Add(new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])
                            });
                        }
                    }
                }
            }
            return View(departamentos);
        }
        public ActionResult Create()
        {
            return View(new Departamento { Activo = true });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_InsertarDepartamento", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                            // Configurar los parámetros
                            cmd.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                            cmd.Parameters.AddWithValue("@Activo", departamento.Activo);

                            conn.Open();
                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                departamento.Id = Convert.ToInt32(result);
                                TempData["Success"] = "Departamento creado exitosamente.";
                                return RedirectToAction("Index");
                            }
                        }
                        catch (SqlException ex)
                        {
                            switch (ex.Message)
                            {
                                case string msg when msg.Contains("nombre del departamento es requerido"):
                                    ModelState.AddModelError("Nombre",
                                        "El nombre del departamento es requerido.");
                                    break;

                                case string msg when msg.Contains("Ya existe un departamento"):
                                    ModelState.AddModelError("Nombre",
                                        "Ya existe un departamento con este nombre.");
                                    break;

                                default:
                                    ModelState.AddModelError("",
                                        "Error al crear el departamento: " + ex.Message);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("",
                                "Error inesperado al crear el departamento: " + ex.Message);
                        }
                    }
                }
            }

            return View(departamento);
        }
     
        public ActionResult Edit(int id)
        {
            Departamento departamento = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Id, Nombre, Activo FROM Departamentos WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            departamento = new Departamento
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Activo = Convert.ToBoolean(reader["Activo"])
                            };
                        }
                    }
                }
            }

            if (departamento == null)
            {
                return HttpNotFound();
            }

            return View(departamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Departamento departamento)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_ActualizarDepartamento", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        try
                        {
                           
                            cmd.Parameters.AddWithValue("@Id", departamento.Id);
                            cmd.Parameters.AddWithValue("@Nombre", departamento.Nombre);
                            cmd.Parameters.AddWithValue("@Activo", departamento.Activo);

                            conn.Open();
                            var result = cmd.ExecuteScalar();

                            if (result != null)
                            {
                                TempData["Success"] = "Departamento actualizado exitosamente.";
                                return RedirectToAction("Index");
                            }
                        }
                        catch (SqlException ex)
                        {
                            switch (ex.Message)
                            {
                                case string msg when msg.Contains("no existe"):
                                    ModelState.AddModelError("",
                                        "El departamento que intenta actualizar no existe.");
                                    break;

                                case string msg when msg.Contains("nombre del departamento es requerido"):
                                    ModelState.AddModelError("Nombre",
                                        "El nombre del departamento es requerido.");
                                    break;

                                case string msg when msg.Contains("Ya existe otro departamento"):
                                    ModelState.AddModelError("Nombre",
                                        "Ya existe otro departamento con este nombre.");
                                    break;

                                case string msg when msg.Contains("tiene empleados asignados"):
                                    ModelState.AddModelError("Activo",
                                        "No se puede desactivar el departamento porque tiene empleados asignados.");
                                    break;

                                default:
                                    ModelState.AddModelError("",
                                        "Error al actualizar el departamento: " + ex.Message);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("",
                                "Error inesperado al actualizar el departamento: " + ex.Message);
                        }
                    }
                }
            }

            return View(departamento);
        }
    }
}