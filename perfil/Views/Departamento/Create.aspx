<%@ Page Title="Crear Departamento" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<perfil.Models.Departamento>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <div class="row">
            <div class="col-md-8 offset-md-2">
                <div class="card">
                    <div class="card-header bg-warning text-white">
                        <h3>Crear Nuevo Departamento</h3>
                    </div>
                    <div class="card-body">
                        <% using (Html.BeginForm("Create", "Departamento", FormMethod.Post)) { %>
                            <%= Html.AntiForgeryToken() %>
                            <%= Html.ValidationSummary(true, "", new { @class = "text-danger" }) %>

                            <div class="form-group mb-3">
                                <%= Html.LabelFor(model => model.Nombre, "Nombre del Departamento", new { @class = "form-label" }) %>
                                <%= Html.TextBoxFor(model => model.Nombre, new { @class = "form-control", placeholder = "Ingrese el nombre del departamento" }) %>
                                <%= Html.ValidationMessageFor(model => model.Nombre, "", new { @class = "text-danger" }) %>
                            </div>

                            <div class="form-group mb-3">
                                <div class="form-check">
                                    <%= Html.CheckBoxFor(model => model.Activo, new { @class = "form-check-input", @checked = "checked" }) %>
                                    <%= Html.LabelFor(model => model.Activo, "Departamento Activo", new { @class = "form-check-label" }) %>
                                </div>
                                <%= Html.ValidationMessageFor(model => model.Activo, "", new { @class = "text-danger" }) %>
                            </div>

                            <div class="form-group mt-4">
                                <input type="submit" value="Crear" class="btn bg-success text-white"  />
                                <%= Html.ActionLink("Regresar a la Lista", "Index", null, new { @class = "btn btn-secondary ms-2" }) %>
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