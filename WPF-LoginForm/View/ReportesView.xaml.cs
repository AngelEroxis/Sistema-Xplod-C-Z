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

        private void ExportarClientes_Click(object sender, RoutedEventArgs e)
        {
            var clientes = dgClientesRpt.ItemsSource.Cast<Cliente>().ToList();
            string ruta = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Reporte_Clientes_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
);
            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                Document doc = new Document(PageSize.A4, 40, 40, 60, 60);
                PdfWriter writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                // Fuente personalizada
                var tituloFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                // Cabecera
                iTextSharp.text.Paragraph titulo = new iTextSharp.text.Paragraph("Xplod C&Z\n", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                iTextSharp.text.Paragraph subtitulo = new iTextSharp.text.Paragraph("Reporte de Clientes\n\n", tituloFont);
                subtitulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(subtitulo);

                iTextSharp.text.Paragraph fecha = new iTextSharp.text.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy - HH:mm:ss}\n\n", normalFont);
                fecha.Alignment = Element.ALIGN_RIGHT;
                doc.Add(fecha);

                // Tabla
                PdfPTable table = new PdfPTable(9);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 5, 15, 10, 10, 20, 10, 20, 15, 10 });

                // Encabezados
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

                // Datos
                foreach (var c in clientes)
                {
                    table.AddCell(new Phrase(c.IdCliente.ToString(), normalFont));
                    table.AddCell(new Phrase(c.Nombre, normalFont));
                    table.AddCell(new Phrase(c.CI, normalFont));
                    table.AddCell(new Phrase(c.NumItem, normalFont));
                    table.AddCell(new Phrase(c.Domicilio, normalFont));
                    table.AddCell(new Phrase(c.Celular, normalFont));
                    table.AddCell(new Phrase(c.EmpresaInstitucion, normalFont));
                    table.AddCell(new Phrase(c.Garante, normalFont));
                    table.AddCell(new Phrase(c.CelGarante, normalFont));
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show("✅ Reporte de clientes generado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        private void ExportarVentas_Click(object s, RoutedEventArgs e)
        {
            var ventas = dgVentasRpt.ItemsSource.Cast<Venta>().ToList();
            string ruta = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                $"Reporte_Ventas_Xplod_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
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

                iTextSharp.text.Paragraph subtitulo = new iTextSharp.text.Paragraph("Reporte de Ventas\n\n", tituloFont);
                subtitulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(subtitulo);

                iTextSharp.text.Paragraph fecha = new iTextSharp.text.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy - HH:mm:ss}\n\n", normalFont);
                fecha.Alignment = Element.ALIGN_RIGHT;
                doc.Add(fecha);

                // Tabla
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10f, 20f, 20f, 15f, 20f, 15f });  // <- aquí el cambio

                string[] headers = { "ID Venta", "Cliente", "Vendedor", "Método de Pago", "Fecha", "Total (Bs)" };
                foreach (var header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(new Chunk(header, subFont)))  // <- aquí cambio
                    {
                        BackgroundColor = new BaseColor(230, 230, 250),
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }

                foreach (var v in ventas)
                {
                    table.AddCell(new Phrase(new Chunk(v.IdVenta.ToString(), normalFont)));
                    table.AddCell(new Phrase(new Chunk(v.IdCliente.ToString(), normalFont)));  // Asumo IdCliente es numérico
                    table.AddCell(new Phrase(new Chunk(v.IdVendedor.ToString(), normalFont)));
                    table.AddCell(new Phrase(new Chunk(v.MetodoPago, normalFont)));
                    table.AddCell(new Phrase(new Chunk(v.Fecha.ToString("dd/MM/yyyy HH:mm"), normalFont)));
                    table.AddCell(new Phrase(new Chunk($"{v.Total:N2}", normalFont)));
                }

                doc.Add(table);
                doc.Close();
            }

            MessageBox.Show("✅ Reporte de ventas generado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
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
