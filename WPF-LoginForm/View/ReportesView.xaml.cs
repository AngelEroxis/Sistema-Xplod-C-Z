using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_LoginForm.Model;
using iTextSharpText = iTextSharp.text;
using iTextSharpPdf = iTextSharp.text.pdf;

using Path = System.IO.Path;
using Rectangle = iTextSharp.text.Rectangle;

using System.Globalization;

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para ReportesView.xaml
    /// </summary>
    public partial class ReportesView : UserControl
    {
        private List<ClienteCreditoViewModel> clientesSeleccionados = new List<ClienteCreditoViewModel>();

        public ReportesView()
        {
            InitializeComponent();
            CargarClientes();
            CargarVentas();
            CargarCreditosPendientes();
            CargarVistaPreviaClientesCredito();

        }

        private void CargarClientes()
        {
            using (var db = new MyDbContext())
            {
                dgClientesRpt.ItemsSource = db.Clientes.ToList();
            }

        }

        private void CargarVentas()
        {
            using (var db = new MyDbContext())
            {
                dgVentasRpt.ItemsSource = db.Ventas.Include(v => v.Vendedor).ToList();
            }
        }

        private void CargarCreditosPendientes()
        {
            using (var db = new MyDbContext())
            {
                var creditos = db.Creditos
                    .Include(c => c.Venta.Cliente)
                    .Include(c => c.Venta.DetalleVentas.Select(d => d.Producto))
                    .Where(c => c.SaldoPendiente > 0)
                    .ToList();

                var creditosVista = creditos.Select(c => new CreditoVista
                {
                    ClienteNombre = c.Venta?.Cliente?.Nombre ?? "",
                    Productos = string.Join(" | ", c.Venta?.DetalleVentas.Select(d => d.Producto?.Nombre ?? "Sin nombre") ?? new List<string>()),
                    Cantidad = c.Venta?.DetalleVentas.Sum(d => d.Cantidad) ?? 0,
                    MontoTotal = c.MontoTotal,
                    SaldoPendiente = c.SaldoPendiente,
                    Cuotas = c.Cuotas,
                    EstadoCredito = c.EstadoCredito,
                    FechaCredito = c.Venta?.Fecha.ToString("dd/MM/yyyy") ?? ""
                }).ToList();

                dgCreditosRpt.ItemsSource = creditosVista;
            }
        }

        private void BtnFiltrarClientes_Click(object sender, RoutedEventArgs e)
        {
            string busqueda = txtBusquedaClientes.Text?.Trim().ToLower();

            using (var db = new MyDbContext())
            {
                var clientesConCreditos = db.Clientes
                    .Where(c => c.Ventas.Any())
                    .Include(c => c.Ventas.Select(v => v.DetalleVentas.Select(d => d.Producto)))
                    .ToList();

                var listaVista = new List<ClienteCreditoViewModel>();

                foreach (var cliente in clientesConCreditos)
                {
                    var ultimaVenta = cliente.Ventas
                        .Where(v => v.DetalleVentas.Any())
                        .OrderByDescending(v => v.Fecha)
                        .FirstOrDefault();

                    if (ultimaVenta == null) continue;

                    var viewModel = new ClienteCreditoViewModel
                    {
                        Nombre = cliente.Nombre,
                        CI = cliente.CI,
                        NumItem = cliente.NumItem,
                        EmpresaInstitucion = cliente.EmpresaInstitucion,
                        Productos = string.Join(", ", ultimaVenta.DetalleVentas.Select(d => d.Producto?.Nombre ?? "Sin nombre")),
                        CantidadTotal = ultimaVenta.DetalleVentas.Sum(d => d.Cantidad),
                        PrecioUnitarioPromedio = ultimaVenta.DetalleVentas.Average(d => d.PrecioUnitario),
                        Total = ultimaVenta.DetalleVentas.Sum(d => d.Cantidad * d.PrecioUnitario),
                        Fecha = ultimaVenta.Fecha
                    };

                    if (string.IsNullOrWhiteSpace(busqueda) || (
                        (viewModel.Nombre ?? "").ToLower().Contains(busqueda) ||
                        (viewModel.CI ?? "").ToLower().Contains(busqueda) ||
                        (viewModel.NumItem ?? "").ToLower().Contains(busqueda) ||
                        (viewModel.EmpresaInstitucion ?? "").ToLower().Contains(busqueda) ||
                        (viewModel.Productos ?? "").ToLower().Contains(busqueda)))
                    {
                        listaVista.Add(viewModel);
                    }
                }

                dgClientesRpt.ItemsSource = listaVista;
            }
        }

        private void BtnAgregarClienteReporte_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientesRpt.SelectedItem is ClienteCreditoViewModel clienteSeleccionado)
            {
                if (!clientesSeleccionados.Any(c => c.CI == clienteSeleccionado.CI))
                {
                    clientesSeleccionados.Add(clienteSeleccionado);
                    dgClientesSeleccionados.ItemsSource = null;
                    dgClientesSeleccionados.ItemsSource = clientesSeleccionados;
                }
                else
                {
                    MessageBox.Show("Este cliente ya fue añadido al reporte.", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente de la tabla superior.", "Atención", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnQuitarCliente_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is ClienteCreditoViewModel cliente)
            {
                clientesSeleccionados.Remove(cliente);
                dgClientesSeleccionados.ItemsSource = null;
                dgClientesSeleccionados.ItemsSource = clientesSeleccionados;
            }
        }


        private void ExportarClientes_Click(object sender, RoutedEventArgs e)
        {
            if (!clientesSeleccionados.Any())
            {
                MessageBox.Show("No hay clientes seleccionados para exportar.", "Sin datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string ruta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Reporte_ClientesCredito_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );

            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4.Rotate(), 40, 40, 60, 60);
                PdfWriter.GetInstance(doc, stream);
                doc.Open();
                AgregarLogoSolo(doc);
                var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                doc.Add(new iTextSharp.text.Paragraph("Xplod C&Z", tituloFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharp.text.Paragraph("Reporte de Clientes a Crédito\n\n", tituloFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharp.text.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy - HH:mm:ss}\n\n", normalFont) { Alignment = Element.ALIGN_RIGHT });

                PdfPTable table = new PdfPTable(10) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 15, 10, 10, 15, 25, 7, 10, 10, 10, 15 });

                string[] headers = {
            "Nombre", "CI", "Nº Ítem", "Empresa/Institución",
            "Producto(s)", "Cant.", "P. Unit.", "Total", "Fecha", "Tipo de Venta"
        };

                foreach (var header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, subFont))
                    {
                        BackgroundColor = new BaseColor(230, 230, 250),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                foreach (var cliente in clientesSeleccionados)
                {
                    table.AddCell(new Phrase(cliente.Nombre ?? "", normalFont));
                    table.AddCell(new Phrase(cliente.CI ?? "", normalFont));
                    table.AddCell(new Phrase(cliente.NumItem ?? "", normalFont));
                    table.AddCell(new Phrase(cliente.EmpresaInstitucion ?? "", normalFont));
                    table.AddCell(new Phrase(cliente.Productos ?? "", normalFont));
                    table.AddCell(new Phrase(cliente.CantidadTotal.ToString(), normalFont));
                    table.AddCell(new Phrase(cliente.PrecioUnitarioPromedio.ToString("N2"), normalFont));
                    table.AddCell(new Phrase(cliente.Total.ToString("N2"), normalFont));
                    table.AddCell(new Phrase(cliente.Fecha.ToString("dd/MM/yyyy HH:mm"), normalFont));
                    table.AddCell(new Phrase("Crédito", normalFont));
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show("✅ Reporte de clientes exportado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }




        public class ClienteCreditoViewModel
        {
            public string Nombre { get; set; }
            public string CI { get; set; }
            public string NumItem { get; set; }
            public string EmpresaInstitucion { get; set; }
            public string Productos { get; set; }
            public int CantidadTotal { get; set; }
            public decimal PrecioUnitarioPromedio { get; set; }
            public decimal Total { get; set; }
            public DateTime Fecha { get; set; }
            public string TipoVenta { get; set; } = "Crédito";
        }

        private void CargarVistaPreviaClientesCredito()
        {
            using (var db = new MyDbContext())
            {
                var clientesConCreditos = db.Clientes
                    .Where(c => c.Ventas.Any())
                    .Include(c => c.Ventas.Select(v => v.DetalleVentas.Select(d => d.Producto)))
                    .ToList();

                var listaVista = new List<ClienteCreditoViewModel>();

                foreach (var cliente in clientesConCreditos)
                {
                    var ultimaVenta = cliente.Ventas
                        .Where(v => v.DetalleVentas.Any())
                        .OrderByDescending(v => v.Fecha)
                        .FirstOrDefault();

                    if (ultimaVenta == null) continue;

                    listaVista.Add(new ClienteCreditoViewModel
                    {
                        Nombre = cliente.Nombre,
                        CI = cliente.CI,
                        NumItem = cliente.NumItem,
                        EmpresaInstitucion = cliente.EmpresaInstitucion,
                        Productos = string.Join(", ", ultimaVenta.DetalleVentas.Select(d => d.Producto?.Nombre ?? "Sin nombre")),
                        CantidadTotal = ultimaVenta.DetalleVentas.Sum(d => d.Cantidad),
                        PrecioUnitarioPromedio = ultimaVenta.DetalleVentas.Average(d => d.PrecioUnitario),
                        Total = ultimaVenta.DetalleVentas.Sum(d => d.Cantidad * d.PrecioUnitario),
                        Fecha = ultimaVenta.Fecha
                    });
                }

                dgClientesRpt.ItemsSource = listaVista;
            }
        }




        public class ReporteCreditoClienteItem
{
    public string Nombre { get; set; }
    public string CI { get; set; }
    public string NumItem { get; set; }
    public string EmpresaInstitucion { get; set; }

    public string Producto { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal Total => Cantidad * PrecioUnitario;

    public DateTime Fecha { get; set; }
    public string FechaTexto => Fecha.ToString("dd/MM/yyyy HH:mm");
}



        private void ExportarVentas_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MyDbContext())
            {
                DateTime desde = dpFechaInicio.SelectedDate?.Date ?? DateTime.MinValue;
                DateTime hasta = dpFechaFin.SelectedDate?.Date.AddDays(1).AddSeconds(-1) ?? DateTime.MaxValue;

                var query = db.Ventas
                    .Include(v => v.DetalleVentas.Select(d => d.Producto))
                    .Include(v => v.Vendedor)
                    .Include(v => v.Cliente)
                    .Where(v => v.Fecha >= desde && v.Fecha <= hasta);

                var lista = new List<VentaReporteItem>();

                foreach (var v in query)
                {
                    foreach (var d in v.DetalleVentas)
                    {
                        lista.Add(new VentaReporteItem
                        {
                            IdVenta = v.IdVenta,
                            Producto = d.Producto.Nombre,
                            Cantidad = d.Cantidad,
                            PrecioUnitario = d.PrecioUnitario,
                            Total = d.Subtotal,
                            Fecha = v.Fecha,
                            MetodoPago = v.MetodoPago,
                            EsCredito = v.Cliente != null
                        });
                    }
                }

                dgVentasRpt.ItemsSource = lista;

                if (!lista.Any())
                {
                    MessageBox.Show("No se encontraron ventas en el rango seleccionado.");
                    return;
                }

                string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Reporte_Ventas_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                GenerarPDFVentas(lista, desde, hasta, lista.Sum(i => i.Total), ruta);
            }
        }


        // Clase auxiliar para el reporte
        public class VentaReporteItem
        {
            public int IdVenta { get; set; }
            public string Producto { get; set; }
            public int Cantidad { get; set; }
            public decimal PrecioUnitario { get; set; }
            public decimal Total { get; set; }
            public DateTime Fecha { get; set; }
            public string MetodoPago { get; set; }
            public bool EsCredito { get; set; }
        }

        private void ActualizarVistaPreviaVentas()
        {
            using (var db = new MyDbContext())
            {
                DateTime fechaInicio = dpFechaInicio.SelectedDate ?? DateTime.MinValue;
                DateTime fechaFin = dpFechaFin.SelectedDate?.AddDays(1).AddSeconds(-1) ?? DateTime.MaxValue;

                var ventas = db.Ventas
                    .Include(v => v.DetalleVentas.Select(d => d.Producto))
                    .Include(v => v.Cliente)
                    .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                    .ToList();

                var vistaPrevia = new List<VentaReporteItem>();

                foreach (var venta in ventas)
                {
                    foreach (var detalle in venta.DetalleVentas)
                    {
                        vistaPrevia.Add(new VentaReporteItem
                        {
                            IdVenta = venta.IdVenta,
                            Producto = detalle.Producto?.Nombre ?? "(Sin producto)",
                            Cantidad = detalle.Cantidad,
                            PrecioUnitario = detalle.PrecioUnitario,
                            Total = detalle.Cantidad * detalle.PrecioUnitario,
                            Fecha = venta.Fecha,
                            MetodoPago = venta.MetodoPago,
                            EsCredito = venta.Cliente != null
                        });
                    }
                }

                dgVentasRpt.ItemsSource = null;
                dgVentasRpt.ItemsSource = vistaPrevia;
            }
        }

        private void dpFechaInicio_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarVistaPreviaVentas();
        }

        private void dpFechaFin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            ActualizarVistaPreviaVentas();
        }
        private void TabVentas_Loaded(object sender, RoutedEventArgs e)
        {
            ActualizarVistaPreviaVentas();
        }




        private void GenerarPDFVentas(List<VentaReporteItem> lista, DateTime desde, DateTime hasta, decimal totalGeneral, string ruta)
        {
            var doc = new Document(PageSize.A4, 40, 40, 60, 60);

            using (var stream = new FileStream(ruta, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, stream);
                doc.Open();
                AgregarLogoSolo(doc);
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normal = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                doc.Add(new iTextSharpText.Paragraph("Reporte de Ventas - Xplod C&Z", titleFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharpText.Paragraph($"Rango: {desde:dd/MM/yyyy} al {hasta:dd/MM/yyyy}", normal)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10f
                });

                var cols = new PdfPTable(8) { WidthPercentage = 100 };
                cols.SetWidths(new float[] { 10, 20, 10, 15, 15, 15, 15, 20 });

                string[] headers = { "ID", "Producto", "Cantidad", "Precio U.", "Total", "Crédito", "Método", "Fecha" };
                foreach (var h in headers)
                    cols.AddCell(new PdfPCell(new Phrase(h, headerFont))
                    {
                        BackgroundColor = new BaseColor(220, 220, 220),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    });

                foreach (var i in lista)
                {
                    cols.AddCell(i.IdVenta.ToString());
                    cols.AddCell(i.Producto);
                    cols.AddCell(i.Cantidad.ToString());
                    cols.AddCell(i.PrecioUnitario.ToString("N2"));
                    cols.AddCell(i.Total.ToString("N2"));
                    cols.AddCell(i.EsCredito ? "Sí" : "No");
                    cols.AddCell(i.MetodoPago);
                    cols.AddCell(i.Fecha.ToString("dd/MM/yyyy HH:mm"));
                }

                doc.Add(cols);

                doc.Add(new iTextSharpText.Paragraph($"\nTOTAL GENERAL: Bs {totalGeneral:N2}", headerFont)
                {
                    Alignment = Element.ALIGN_RIGHT
                });

                decimal dia = lista.Where(i => i.Fecha.Date == DateTime.Today).Sum(i => i.Total);
                decimal mes = lista.Where(i => i.Fecha.Month == DateTime.Today.Month && i.Fecha.Year == DateTime.Today.Year).Sum(i => i.Total);
                decimal año = lista.Where(i => i.Fecha.Year == DateTime.Today.Year).Sum(i => i.Total);

                doc.Add(new iTextSharpText.Paragraph($"\nTotal Hoy: Bs {dia:N2}", normal));
                doc.Add(new iTextSharpText.Paragraph($"Total Mes: Bs {mes:N2}", normal));
                doc.Add(new iTextSharpText.Paragraph($"Total Año: Bs {año:N2}", normal));

                doc.Close();
            }

            MessageBox.Show($"✅ PDF generado correctamente:\n{ruta}", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }






        private void AgregarLogoSolo(Document doc)
        {
            var logoTabla = new PdfPTable(1) { WidthPercentage = 100 };

            string logoPath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Images", "logo.png");

            if (File.Exists(logoPath))
            {
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(logoPath);
                img.ScaleAbsolute(50, 50);
                logoTabla.AddCell(new PdfPCell(img) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_LEFT });
            }
            else
            {
                logoTabla.AddCell(new PdfPCell(new Phrase("Sin logo")) { Border = Rectangle.NO_BORDER });
            }

            doc.Add(logoTabla);
            doc.Add(new iTextSharpText.Paragraph("\n")); // Espacio después del logo
        }




        private void ExportarCreditos_Click(object s, RoutedEventArgs e)
        {
            List<Credito> creditos;

            using (var db = new MyDbContext())
            {
                creditos = db.Creditos
                    .Include(c => c.Venta.Cliente)
                    .Include(c => c.Venta.DetalleVentas.Select(d => d.Producto))
                    .Where(c => c.SaldoPendiente > 0)
                    .ToList();
            }

            string ruta = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Reporte_Creditos_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );

            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4.Rotate(), 40, 40, 60, 60);
                PdfWriter.GetInstance(doc, stream);
                doc.Open();
                AgregarLogoSolo(doc);

                var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                doc.Add(new iTextSharpText.Paragraph("Xplod C&Z", tituloFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharpText.Paragraph("Reporte de Créditos Pendientes\n\n", tituloFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharpText.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy - HH:mm:ss}\n\n", normalFont) { Alignment = Element.ALIGN_RIGHT });

                PdfPTable table = new PdfPTable(8);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 20, 30, 10, 15, 15, 10, 15, 15 });

                string[] headers = {
            "Cliente",
            "Producto(s)",
            "Cantidad",
            "Total Crédito (Bs)",
            "Saldo Pendiente (Bs)",
            "Cuotas",
            "Estado",
            "Fecha"
        };

                foreach (var header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, subFont))
                    {
                        BackgroundColor = new BaseColor(230, 230, 250),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                foreach (var c in creditos)
                {
                    var cliente = c.Venta?.Cliente;
                    var productos = string.Join(" | ", c.Venta?.DetalleVentas.Select(d => d.Producto?.Nombre ?? "Sin nombre") ?? new List<string>());
                    var cantidad = c.Venta?.DetalleVentas.Sum(d => d.Cantidad) ?? 0;
                    var fecha = c.Venta?.Fecha.ToString("dd/MM/yyyy") ?? "";

                    table.AddCell(new Phrase(cliente?.Nombre ?? "", normalFont));
                    table.AddCell(new Phrase(productos, normalFont));
                    table.AddCell(new Phrase(cantidad.ToString(), normalFont));
                    table.AddCell(new Phrase($"{c.MontoTotal:N2}", normalFont));
                    table.AddCell(new Phrase($"{c.SaldoPendiente:N2}", normalFont));
                    table.AddCell(new Phrase(c.Cuotas.ToString(), normalFont));
                    table.AddCell(new Phrase(c.EstadoCredito ?? "", normalFont));
                    table.AddCell(new Phrase(fecha, normalFont));
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show("✅ Reporte de créditos generado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public class CreditoVista
        {
            public string ClienteNombre { get; set; }
            public string Productos { get; set; }
            public int Cantidad { get; set; }
            public decimal MontoTotal { get; set; }
            public decimal SaldoPendiente { get; set; }
            public int Cuotas { get; set; }
            public string EstadoCredito { get; set; }
            public string FechaCredito { get; set; }
        }



    }
}
