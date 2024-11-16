<%@ Page Title="Editar Empleado" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<perfil.Models.Empleado>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-md-10 offset-md-1">
                <div class="card">
                    <div class="card-header bg-dark text-white">
                        <h3 class="mb-0">Editar Empleado</h3>
                    </div>
                    <div class="card-body">
                        <% using (Html.BeginForm("Edit", "Empleado", FormMethod.Post)) { %>
                            <%= Html.AntiForgeryToken() %>
                            <%= Html.ValidationSummary(true, "", new { @class = "text-danger" }) %>
                            <%= Html.HiddenFor(m => m.Id) %>

                            <div class="row">
                                <!-- Primera columna -->
                                <div class="col-md-6">
                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.Nombres, "Nombres", new { @class = "form-label" }) %>
                                        <%= Html.TextBoxFor(m => m.Nombres, new { @class = "form-control" }) %>
                                        <%= Html.ValidationMessageFor(m => m.Nombres, "", new { @class = "text-danger" }) %>
                                    </div>

                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.FechaNacimiento, "Fecha de Nacimiento", new { @class = "form-label" }) %>
                                        <%= Html.TextBoxFor(m => m.FechaNacimiento, "{0:yyyy-MM-dd}", new { @class = "form-control", type = "date" }) %>
                                        <%= Html.ValidationMessageFor(m => m.FechaNacimiento, "", new { @class = "text-danger" }) %>
                                    </div>

                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.FechaIngreso, "Fecha de Ingreso", new { @class = "form-label" }) %>
                                        <%= Html.TextBoxFor(m => m.FechaIngreso, "{0:yyyy-MM-dd}", new { @class = "form-control", type = "date" }) %>
                                        <%= Html.ValidationMessageFor(m => m.FechaIngreso, "", new { @class = "text-danger" }) %>
                                    </div>

                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.NIT, "NIT", new { @class = "form-label" }) %>
                                        <%= Html.TextBoxFor(m => m.NIT, new { @class = "form-control" }) %>
                                        <%= Html.ValidationMessageFor(m => m.NIT, "", new { @class = "text-danger" }) %>
                                    </div>
                                </div>

                                <!-- Segunda columna -->
                                <div class="col-md-6">
                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.DPI, "DPI", new { @class = "form-label" }) %>
                                        <%= Html.TextBoxFor(m => m.DPI, new { @class = "form-control" }) %>
                                        <%= Html.ValidationMessageFor(m => m.DPI, "", new { @class = "text-danger" }) %>
                                    </div>

                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.Genero, "Género", new { @class = "form-label" }) %>
                                        <%= Html.DropDownListFor(m => m.Genero, 
                                            new SelectList(new[] { "Masculino", "Femenino" }), 
                                            "Seleccione género...", 
                                            new { @class = "form-select" }) %>
                                        <%= Html.ValidationMessageFor(m => m.Genero, "", new { @class = "text-danger" }) %>
                                    </div>

                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.Direccion, "Dirección", new { @class = "form-label" }) %>
                                        <%= Html.TextBoxFor(m => m.Direccion, new { @class = "form-control" }) %>
                                        <%= Html.ValidationMessageFor(m => m.Direccion, "", new { @class = "text-danger" }) %>
                                    </div>

                                    <div class="form-group mb-3">
                                        <%= Html.LabelFor(m => m.DepartamentoId, "Departamento", new { @class = "form-label" }) %>
                                        <%= Html.DropDownListFor(m => m.DepartamentoId, 
                                            (SelectList)ViewBag.Departamentos,
                                            "Seleccione departamento...", 
                                            new { @class = "form-select" }) %>
                                        <%= Html.ValidationMessageFor(m => m.DepartamentoId, "", new { @class = "text-danger" }) %>
                                    </div>
                                </div>
                            </div>

                            <div class="row mt-4">
                                <div class="col-12 text-center">
                                    <input type="submit" value="Guardar" class="btn btn-primary px-4" />
                                    <%= Html.ActionLink("Regresar a la Lista", "Index", null, new { @class = "btn btn-secondary ms-2" }) %>
                                </div>
                            </div>
                        <% } %>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="<%: Url.Content("~/Scripts/jquery.validate.min.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/jquery.validate.unobtrusive.min.js") %>" type="text/javascript"></script>
</asp:Content>