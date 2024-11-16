<%@ Page Title="Reporte de Empleados por Departamento" Language="C#" MasterPageFile="~/Site.Master" %>
<%@ Import Namespace="perfil.Models.ViewModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="mt-4 mb-4">Reporte de Empleados por Departamento</h2>

        <div class="row mb-3">
            <div class="col-md-12">
                <% foreach (var departamento in (List<EmpleadoDepartamentoViewModel>)Model) { %>
                    <div class="card mb-4">
                        <div class="card-header  bg-warning text-white">
                            <div class="row">
                                <div class="col-md-8">
                                    <h4>
                                        <%= departamento.NombreDepartamento %>
                                        <span class="badge <%= departamento.DepartamentoActivo ? "bg-success" : "bg-danger" %>">
                                            <%= departamento.DepartamentoActivo ? "Activo" : "Inactivo" %>
                                        </span>
                                    </h4>
                                </div>
                                <div class="col-md-4 text-end">
                                    <span class="badge bg-primary">
                                        Total Empleados: <%= departamento.TotalEmpleados %>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="card-body">
                            <div class="table-responsive">
                                <% if (departamento.Empleados != null && departamento.Empleados.Any()) { %>
                                    <table class="table table table-bordered table-hover">
                                        <thead class="table-secondary">
                                            <tr>
                                                <th>Nombres</th>
                                                <th>DPI</th>
                                                <th>NIT</th>
                                                <th>Edad</th>
                                                 <th>Fecha Ingreso</th>
                                                <th>Tiempo Laborando</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <% foreach (var empleado in departamento.Empleados) { %>
                                                <tr>
                                                    <td><%= empleado.Nombres %></td>
                                                    <td><%= empleado.DPI %></td>
                                                    <td><%= empleado.NIT %></td>
                                                    <td><%= empleado.Edad %> años</td>
                                                    <td><%= empleado.FechaIngreso.ToString("dd/MM/yyyy") %></td>
                                                    <td><%= empleado.TiempoLaborando %></td>
                                                </tr>
                                            <% } %>
                                        </tbody>
                                    </table>
                                <% } else { %>
                                    <div class="alert alert-info">
                                        No hay empleados registrados en este departamento.
                                    </div>
                                <% } %>
                            </div>
                        </div>
                    </div>
                <% } %>

                <% if (Model == null || !((List<EmpleadoDepartamentoViewModel>)Model).Any()) { %>
                    <div class="alert alert-warning">
                        No se encontraron departamentos o empleados para mostrar.
                    </div>
                <% } %>
            </div>
        </div>
        <div class="row mb-3">
            <div class="col-md-12">
                <button class="btn btn-primary ms-2" onclick="window.print(); return false;">
                    Imprimir Reporte
                </button>
            </div>
        </div>
    </div>

    <style type="text/css">
        @media print {
            .btn, .no-print {
                display: none !important;
            }
            .card {
                border: none !important;
            }
            .table {
                border-color: #dee2e6 !important;
            }
        }
        .badge {
            font-size: 0.9em;
        }
        .table > :not(caption) > * > * {
            padding: 0.5rem;
        }
        .card {
            margin-bottom: 1.5rem;
        }
        .table-responsive {
            min-height: 0.01%;
            overflow-x: auto;
        }
    </style>

    <!-- Scripts -->
    <script type="text/javascript">
        $(document).ready(function () {
            var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
            var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
                return new bootstrap.Tooltip(tooltipTriggerEl)
            });
            function adjustColumnWidths() {
                $('.table').each(function () {
                    var $table = $(this);
                    var totalWidth = $table.width();
                    var columnCount = $table.find('th').length;
                    var columnWidth = Math.floor(totalWidth / columnCount);

                    $table.find('th, td').css('min-width', columnWidth + 'px');
                });
            }
            adjustColumnWidths();
            $(window).resize(adjustColumnWidths);
        });
    </script>
</asp:Content>