using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.View
{
    public partial class ComprasRealizadasWindow : Window
    {
        private readonly MyDbContext db = new MyDbContext();

        public ComprasRealizadasWindow()
        {
            InitializeComponent();
            CargarCompras();
        }

        private void CargarCompras()
        {
            var compras = db.Compras
                .Include(c => c.Proveedor)
                .Include(c => c.DetalleCompras.Select(d => d.Producto))
                .ToList();

            dgCompras.ItemsSource = compras;
        }

        private void dgCompras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var compra = dgCompras.SelectedItem as Compra;
            if (compra != null)
            {
                dgDetalleCompra.ItemsSource = compra.DetalleCompras.ToList();
            }
            else
            {
                dgDetalleCompra.ItemsSource = null;
            }
        }

        private void BtnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            var compra = dgCompras.SelectedItem as Compra;
            if (compra == null)
            {
                MessageBox.Show("Selecciona una compra primero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            MessageBox.Show($"Compra #{compra.IdCompra} - {compra.Proveedor.Nombre} - Total: {compra.Total:N2}", "Detalles", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnGenerarPdf_Click(object sender, RoutedEventArgs e)
        {
            var compra = dgCompras.SelectedItem as Compra;
            if (compra == null)
            {
                MessageBox.Show("Selecciona una compra primero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var nombreArchivo = $"Compra_{compra.IdCompra}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var ruta = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nombreArchivo);

                using (var fs = new FileStream(ruta, FileMode.Create))
                using (var doc = new Document(PageSize.A4, 25, 25, 30, 30))
                {
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    doc.Add(new Paragraph($"Detalle de Compra #{compra.IdCompra}") { Alignment = Element.ALIGN_CENTER, SpacingAfter = 15f });
                    doc.Add(new Paragraph($"Proveedor: {compra.Proveedor.Nombre}"));
                    doc.Add(new Paragraph($"Fecha:      {compra.Fecha:dd/MM/yyyy}"));
                    doc.Add(new Paragraph($"Total Bs:   {compra.Total:N2}"));
                    doc.Add(new Paragraph(" "));

                    var tabla = new PdfPTable(4) { WidthPercentage = 100 };
                    tabla.AddCell("Producto");
                    tabla.AddCell("Cantidad");
                    tabla.AddCell("Precio Unit.");
                    tabla.AddCell("Subtotal");

                    foreach (var det in compra.DetalleCompras)
                    {
                        tabla.AddCell(det.Producto.Nombre);
                        tabla.AddCell(det.Cantidad.ToString());
                        tabla.AddCell(det.PrecioUnitario.ToString("N2"));
                        tabla.AddCell(det.SubTotal.ToString("N2"));
                    }

                    doc.Add(tabla);
                    doc.Close();
                }

                MessageBox.Show($"PDF generado en: {Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}", "PDF creado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
