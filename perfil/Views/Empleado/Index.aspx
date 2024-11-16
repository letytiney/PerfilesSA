<%@ Page Title="Empleados" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<IEnumerable<perfil.Models.Empleado>>" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Lista de Empleados</h2>
        
        <div class="mb-3">
            <%= Html.ActionLink("Crear Nuevo Empleado", "Create", "Empleado", null, new { @class = "btn btn-success" }) %>
        </div>

        <table class="table table table-bordered  table-hover">
            <thead class="table-dark">
                <tr>
                    <th>Nombres</th>
                    <th>DPI</th>
                    <th>Fecha Nacimiento</th>
                    <th>Género</th>
                    <th>Fecha Ingreso</th>
                    <th>Dirección</th>
                    <th>NIT</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var item in Model) { %>
                    <tr>
                        <td><%: item.Nombres %></td>
                        <td><%: item.DPI %></td>
                        <td><%: item.FechaNacimiento.ToShortDateString() %></td>
                        <td><%: item.Genero %></td>
                        <td><%: item.FechaIngreso.ToShortDateString() %></td>
                        <td><%: item.Direccion %></td>
                        <td><%: item.NIT %></td>
                        <td>
                            <%= Html.ActionLink(" ", "Edit", new { id=item.Id }, new { @class = "btn btn-warning btn-sm bi bi-pencil-square", title="Editar" }) %>
                            <%= Html.ActionLink(" ", "Details", new { id=item.Id }, new { @class = "btn btn-info btn-sm bi bi-info-circle ms-1", title="Detalles" }) %>
                        </td>
                    </tr>
                <% } %>
            </tbody>
        </table>
    </div>
</asp:Content>