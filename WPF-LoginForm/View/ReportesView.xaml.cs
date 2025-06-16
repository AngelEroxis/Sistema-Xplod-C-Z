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

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para ReportesView.xaml
    /// </summary>
    public partial class ReportesView : UserControl
    {
        public ReportesView()
        {
            InitializeComponent();
            CargarClientes();
            CargarVentas();
            CargarCreditosPendientes();
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
                dgCreditosRpt.ItemsSource = db.Creditos
                .Include(c => c.Cliente)
                .Where(c => c.SaldoPendiente > 0)
                .ToList();
            }
        }
        private void BtnFiltrarClientes_Click(object sender, RoutedEventArgs e)
        {
            string busqueda = txtBusquedaClientes.Text?.Trim().ToLower();

            using (var db = new MyDbContext())
            {
                var query = db.Clientes.AsQueryable();

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    query = query.Where(c =>
                        (c.Nombre ?? "").ToLower().Contains(busqueda) ||
                        (c.CI ?? "").ToLower().Contains(busqueda) ||
                        (c.NumItem ?? "").ToLower().Contains(busqueda) ||
                        (c.Domicilio ?? "").ToLower().Contains(busqueda) ||
                        (c.Celular ?? "").ToLower().Contains(busqueda) ||
                        (c.EmpresaInstitucion ?? "").ToLower().Contains(busqueda) ||
                        (c.Garante ?? "").ToLower().Contains(busqueda) ||
                        (c.CelGarante ?? "").ToLower().Contains(busqueda));
                }

                var clientesFiltrados = query.ToList();
                dgClientesRpt.ItemsSource = clientesFiltrados;
            }
        }


        private void ExportarClientes_Click(object sender, RoutedEventArgs e)
        {
            var clientesFiltrados = dgClientesRpt.ItemsSource.Cast<Cliente>().ToList();

            if (!clientesFiltrados.Any())
            {
                MessageBox.Show("No hay clientes para exportar.", "Sin datos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string ruta = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Reporte_Clientes_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );

            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 40, 40, 60, 60);
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                doc.Add(new iTextSharp.text.Paragraph("Xplod C&Z", tituloFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharp.text.Paragraph("Reporte de Clientes\n\n", tituloFont) { Alignment = Element.ALIGN_CENTER });
                doc.Add(new iTextSharp.text.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy - HH:mm:ss}\n\n", normalFont) { Alignment = Element.ALIGN_RIGHT });

                PdfPTable table = new PdfPTable(9) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 5, 15, 10, 10, 20, 10, 20, 15, 10 });

                string[] headers = { "ID", "Nombre", "CI", "Nº Ítem", "Domicilio", "Celular", "Empresa/Institución", "Garante", "Cel. Garante" };
                foreach (var header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, subFont))
                    {
                        BackgroundColor = new BaseColor(230, 230, 250),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                foreach (var c in clientesFiltrados)
                {
                    table.AddCell(new Phrase(c.IdCliente.ToString(), normalFont));
                    table.AddCell(new Phrase(c.Nombre ?? "", normalFont));
                    table.AddCell(new Phrase(c.CI ?? "", normalFont));
                    table.AddCell(new Phrase(c.NumItem ?? "", normalFont));
                    table.AddCell(new Phrase(c.Domicilio ?? "", normalFont));
                    table.AddCell(new Phrase(c.Celular ?? "", normalFont));
                    table.AddCell(new Phrase(c.EmpresaInstitucion ?? "", normalFont));
                    table.AddCell(new Phrase(c.Garante ?? "", normalFont));
                    table.AddCell(new Phrase(c.CelGarante ?? "", normalFont));
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show("✅ Reporte de clientes generado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }




        private void ExportarVentas_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MyDbContext())
            {
                DateTime fechaInicio = dpFechaInicio.SelectedDate ?? DateTime.MinValue;
                DateTime fechaFin = dpFechaFin.SelectedDate?.AddDays(1).AddSeconds(-1) ?? DateTime.MaxValue;

                var ventas = db.Ventas
                    .Include(v => v.DetalleVentas.Select(d => d.Producto))
                    .Include(v => v.Vendedor)
                    .Include(v => v.Cliente)
                    .Where(v => v.Fecha >= fechaInicio && v.Fecha <= fechaFin)
                    .ToList();

                if (!ventas.Any())
                {
                    MessageBox.Show("No se encontraron ventas en el rango de fechas seleccionado.", "Sin resultados", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                string ruta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Reporte_Ventas_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
                using (FileStream stream = new FileStream(ruta, FileMode.Create))
                {
                    var doc = new iTextSharpText.Document(iTextSharpText.PageSize.A4, 40, 40, 60, 60);
                    iTextSharpPdf.PdfWriter.GetInstance(doc, stream);
                    doc.Open();

                    var tituloFont = iTextSharpText.FontFactory.GetFont(iTextSharpText.FontFactory.HELVETICA_BOLD, 16);
                    var subFont = iTextSharpText.FontFactory.GetFont(iTextSharpText.FontFactory.HELVETICA_BOLD, 12);
                    var normalFont = iTextSharpText.FontFactory.GetFont(iTextSharpText.FontFactory.HELVETICA, 10);

                    // Título y Fecha
                    var titulo = new iTextSharpText.Paragraph("Reporte de Ventas - Xplod C&Z", tituloFont) { Alignment = iTextSharpText.Element.ALIGN_CENTER };
                    doc.Add(titulo);

                    doc.Add(new iTextSharpText.Paragraph($"Desde: {fechaInicio:dd/MM/yyyy}    Hasta: {fechaFin:dd/MM/yyyy}", normalFont)
                    {
                        Alignment = iTextSharpText.Element.ALIGN_CENTER,
                        SpacingAfter = 10f
                    });

                    var table = new iTextSharpPdf.PdfPTable(6)
                    {
                        WidthPercentage = 100
                    };
                    table.SetWidths(new float[] { 10f, 20f, 20f, 15f, 20f, 15f });

                    string[] headers = { "ID Venta", "Producto", "Cantidad", "Precio Unitario", "Método de Pago", "Fecha" };
                    foreach (var header in headers)
                    {
                        var cell = new iTextSharpPdf.PdfPCell(new iTextSharpText.Phrase(header, subFont))
                        {
                            BackgroundColor = new iTextSharpText.BaseColor(220, 220, 250),
                            HorizontalAlignment = iTextSharpText.Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);
                    }

                    foreach (var venta in ventas)
                    {
                        foreach (var detalle in venta.DetalleVentas)
                        {
                            table.AddCell(venta.IdVenta.ToString());
                            table.AddCell(detalle.Producto.Nombre);
                            table.AddCell(detalle.Cantidad.ToString());
                            table.AddCell(detalle.PrecioUnitario.ToString("N2"));
                            table.AddCell(venta.MetodoPago);
                            table.AddCell(venta.Fecha.ToString("dd/MM/yyyy HH:mm"));
                        }
                    }

                    doc.Add(table);

                    // Totales generales
                    decimal total = ventas.Sum(v => v.Total);
                    var totalFinal = new iTextSharpText.Paragraph($"Total General: Bs {total:N2}", subFont)
                    {
                        Alignment = iTextSharpText.Element.ALIGN_RIGHT,
                        SpacingBefore = 15f
                    };
                    doc.Add(totalFinal);

                    doc.Close();
                }

                MessageBox.Show("✅ Reporte de ventas generado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }





        private void ExportarCreditos_Click(object s, RoutedEventArgs e)
        {
            var creditos = dgCreditosRpt.ItemsSource.Cast<Credito>().ToList();
            string ruta = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Reporte_Creditos_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            );

            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 40, 40, 60, 60);
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Título y subtítulo
                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Xplod C&Z\n", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                iTextSharp.text.Paragraph subtitulo = new iTextSharp.text.Paragraph("Reporte de Créditos\n\n", tituloFont);
                subtitulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(subtitulo);

                iTextSharp.text.Paragraph fecha = new iTextSharp.text.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy - HH:mm:ss}\n\n", normalFont);
                fecha.Alignment = Element.ALIGN_RIGHT;
                doc.Add(fecha);

                // Tabla
                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10, 20, 10, 15, 15, 10, 20 });

                string[] headers = { "ID Crédito", "Cliente", "ID Venta", "Monto Total (Bs)", "Saldo Pendiente (Bs)", "Cuotas", "Estado" };
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
                    table.AddCell(new Phrase(c.IdCredito.ToString(), normalFont));
                    table.AddCell(new Phrase(new Chunk(c.IdCliente.ToString(), normalFont)));
                    table.AddCell(new Phrase(c.IdVenta.ToString(), normalFont));
                    table.AddCell(new Phrase($"{c.MontoTotal:N2}", normalFont));
                    table.AddCell(new Phrase($"{c.SaldoPendiente:N2}", normalFont));
                    table.AddCell(new Phrase(c.Cuotas.ToString(), normalFont));
                    table.AddCell(new Phrase(c.EstadoCredito, normalFont));
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show("✅ Reporte de créditos generado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
