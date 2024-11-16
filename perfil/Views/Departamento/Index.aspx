<%@ Page Title="Departamentos" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<IEnumerable<perfil.Models.Departamento>>" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Lista de Departamentos</h2>
        
        <div class="mb-3">
            <%= Html.ActionLink("Crear Nuevo Departamento", "Create", "Departamento", null, new { @class = "btn btn-success" }) %>
        </div>
        <table class="table table table-bordered table-hover">
            <thead class="table-dark">
                <tr>
                    <th>Nombre</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody>
                <% foreach (var item in Model) { %>
                    <tr>
                        <td><%: item.Nombre %></td>
                        <td><%: item.Activo ? "Activo" : "Inactivo" %></td>
                       <td>
                         <%= Html.ActionLink(" ", "Edit", new { id=item.Id }, new { @class = "btn btn-warning btn-sm bi bi-pencil-square", title="Editar" }) %>
                       </td>
                    </tr>
                <% } %>
            </tbody>
        </table>
    </div>
</asp:Content>