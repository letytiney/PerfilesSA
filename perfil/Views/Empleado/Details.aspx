<%@ Page Title="Detalles del Empleado" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<perfil.Models.Empleado>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-md-8 offset-md-2">
                <div class="card">
                    <div class="card-header bg-warning text-white">
                        <h3 class="mb-0">Detalles del Empleado</h3>
                    </div>
                    <div class="card-body">
                        <dl class="row">
                            <dt class="col-sm-3">Nombres:</dt>
                            <dd class="col-sm-9"><%: Model.Nombres %></dd>

                            <dt class="col-sm-3">DPI:</dt>
                            <dd class="col-sm-9"><%: Model.DPI %></dd>

                            <dt class="col-sm-3">Fecha de Nacimiento:</dt>
                            <dd class="col-sm-9"><%: Model.FechaNacimiento.ToShortDateString() %></dd>

                            <dt class="col-sm-3">Edad:</dt>
                            <dd class="col-sm-9"><%: Model.Edad %> años</dd>

                            <dt class="col-sm-3">Género:</dt>
                            <dd class="col-sm-9"><%: Model.Genero %></dd>

                            <dt class="col-sm-3">Fecha de Ingreso:</dt>
                            <dd class="col-sm-9"><%: Model.FechaIngreso.ToShortDateString() %></dd>

                            <dt class="col-sm-3">Dirección:</dt>
                            <dd class="col-sm-9"><%: Model.Direccion %></dd>

                            <dt class="col-sm-3">NIT:</dt>
                            <dd class="col-sm-9"><%: Model.NIT %></dd>

                            <dt class="col-sm-3">Departamento:</dt>
                            <dd class="col-sm-9">
                                <% if (!string.IsNullOrEmpty(Model.DepartamentoNombre)) { %>
                                    <span class="badge bg-info"><%: Model.DepartamentoNombre %></span>
                                <% } else { %>
                                    <span class="badge bg-secondary">No asignado</span>
                                <% } %>
                            </dd>

                            <dt class="col-sm-3">Tiempo de Servicio:</dt>
                            <dd class="col-sm-9">
                                <%  
                                    var tiempoServicio = DateTime.Now - Model.FechaIngreso;
                                    var años = Math.Floor(tiempoServicio.TotalDays / 365);
                                    var meses = Math.Floor((tiempoServicio.TotalDays % 365) / 30);
                                %>
                                <%: años %> años y <%: meses %> meses
                            </dd>
                        </dl>


                        <div class="card mt-4">
                            <div class="card-header bg-light">
                                <h4 class="mb-0">Información Adicional</h4>
                            </div>
                            <div class="card-body">
                                <div class="row">
                               <div class="col-md-6">
                                    <div class="alert alert-info">
                                        <strong>Estado del Departamento:</strong> 
                                        <% if (Model.DepartamentoId.HasValue) { %>
                                            <% if (Model.DepartamentoActivo.HasValue && Model.DepartamentoActivo.Value) { %>
                                                <span class="badge bg-success">Departamento Activo</span>
                                            <% } else { %>
                                                <span class="badge bg-danger">Departamento Inactivo</span>
                                            <% } %>
                                        <% } else { %>
                                            <span class="badge bg-secondary">Sin Departamento Asignado</span>
                                        <% } %>
                                    </div>
                                </div>
                                    <div class="col-md-6">
                                        <div class="alert alert-info">
                                            <strong>Antigüedad:</strong>
                                            <% if (años < 1) { %>
                                                <span class="badge bg-primary">Nuevo Ingreso</span>
                                            <% } else if (años < 5) { %>
                                                <span class="badge bg-info">Regular</span>
                                            <% } else { %>
                                                <span class="badge bg-success">Veterano</span>
                                            <% } %>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mt-4">
                            <div class="btn-group">
                                <%= Html.ActionLink("Editar", "Edit", new { id = Model.Id }, new { @class = "btn btn-warning" }) %>
                                <%= Html.ActionLink("Regresar a la Lista", "Index", null, new { @class = "btn btn-secondary" }) %>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        dt {
            font-weight: bold;
            margin-bottom: 0.5rem;
        }
        dd {
            margin-bottom: 0.5rem;
        }
        .badge {
            font-size: 0.9em;
            padding: 0.5em 1em;
        }
    </style>
</asp:Content>