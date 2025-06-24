using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
                var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nombreArchivo);

                using (var fs = new FileStream(ruta, FileMode.Create))
                using (var doc = new Document(PageSize.A4, 25, 25, 30, 30))
                {
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // 🔹 LOGO + TÍTULO
                    var logoTitulo = new PdfPTable(2) { WidthPercentage = 100 };
                    logoTitulo.SetWidths(new float[] { 15, 85 });

                    string logoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "logo.png");

                    if (File.Exists(logoPath))
                    {
                        // En vez de usar "Image" directamente, usá el namespace completo para iTextSharp:
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(logoPath);

                        img.ScaleAbsolute(50, 50);
                        logoTitulo.AddCell(new PdfPCell(img) { Border = Rectangle.NO_BORDER, Rowspan = 2 });
                    }
                    else
                    {
                        logoTitulo.AddCell(new PdfPCell(new Phrase("Sin logo")) { Border = Rectangle.NO_BORDER });
                    }

                    logoTitulo.AddCell(new PdfPCell(new Phrase("Compra de Productos para Xplod C&Z", FontFactory.GetFont("Arial", 16, Font.BOLD)))
                    {
                        Border = Rectangle.NO_BORDER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    });

                    logoTitulo.AddCell(new PdfPCell(new Phrase(compra.Fecha.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 10)))
                    {
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_LEFT
                    });

                    doc.Add(logoTitulo);
                    doc.Add(new Paragraph("\n"));

                    // 🔹 DETALLES DE LA COMPRA
                    var vendedor = SesionActual.UsuarioLogueado?.Vendedor;

                    string nombreVendedor = vendedor != null ? vendedor.Nombre : "Ortega Duran Caleb Alejandro";
                    string celularVendedor = vendedor != null ? vendedor.Celular : "68420092";

                    var detallesCompra = new PdfPTable(2) { WidthPercentage = 100 };
                    detallesCompra.SetWidths(new float[] { 50, 50 });

                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Nro. Compra: {compra.IdCompra}", FontFactory.GetFont("Arial", 10))) { Border = Rectangle.NO_BORDER });
                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Proveedor: {compra.Proveedor.Nombre}", FontFactory.GetFont("Arial", 10))) { Border = Rectangle.NO_BORDER });
                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Contacto: {compra.Proveedor.Contacto}", FontFactory.GetFont("Arial", 10))) { Border = Rectangle.NO_BORDER });
                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Dueño: {nombreVendedor}", FontFactory.GetFont("Arial", 10))) { Border = Rectangle.NO_BORDER });
                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Celular: {celularVendedor}", FontFactory.GetFont("Arial", 10))) { Border = Rectangle.NO_BORDER });

                    doc.Add(detallesCompra);
                    doc.Add(new Paragraph("\n"));

                    // 🔹 TABLA DE PRODUCTOS
                    var tablaProductos = new PdfPTable(6) { WidthPercentage = 100 };
                    tablaProductos.SetWidths(new float[] { 10, 30, 20, 10, 15, 15 });

                    string[] headers = { "Nro", "Producto", "Unidad Medida", "Cantidad", "Precio Unitario", "Total" };
                    foreach (var header in headers)
                    {
                        tablaProductos.AddCell(new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 10, Font.BOLD)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = BaseColor.LIGHT_GRAY
                        });
                    }

                    decimal totalCompra = 0;
                    int nro = 1;
                    foreach (var detalle in compra.DetalleCompras)
                    {
                        tablaProductos.AddCell(new PdfPCell(new Phrase(nro.ToString())) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Producto.Nombre)) { HorizontalAlignment = Element.ALIGN_LEFT });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Producto.UnidadMedida)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Cantidad.ToString())) { HorizontalAlignment = Element.ALIGN_RIGHT });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.PrecioUnitario.ToString("N2"))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                        tablaProductos.AddCell(new PdfPCell(new Phrase((detalle.PrecioUnitario * detalle.Cantidad).ToString("N2"))) { HorizontalAlignment = Element.ALIGN_RIGHT });


                        totalCompra += detalle.PrecioUnitario * detalle.Cantidad;

                        nro++;
                    }

                    doc.Add(tablaProductos);
                    doc.Add(new Paragraph("\n"));

                    // 🔹 TOTAL
                    var totalCompraParrafo = new Paragraph($"Total de la compra: {totalCompra:N2}", FontFactory.GetFont("Arial", 12, Font.BOLD))
                    {
                        Alignment = Element.ALIGN_RIGHT
                    };
                    doc.Add(totalCompraParrafo);

                    doc.Add(new Paragraph("\n\n\n"));

                    // 🔹 FIRMA DEL VENDEDOR
                    var firmaTabla = new PdfPTable(1) { WidthPercentage = 40, HorizontalAlignment = Element.ALIGN_LEFT };
                    firmaTabla.AddCell(new PdfPCell(new Phrase("_______________________________")) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    firmaTabla.AddCell(new PdfPCell(new Phrase($"{nombreVendedor}", FontFactory.GetFont("Arial", 10))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });
                    firmaTabla.AddCell(new PdfPCell(new Phrase("Firma del Vendedor", FontFactory.GetFont("Arial", 8, Font.ITALIC))) { Border = Rectangle.NO_BORDER, HorizontalAlignment = Element.ALIGN_CENTER });

                    doc.Add(firmaTabla);

                    doc.Close();
                }

                MessageBox.Show($"PDF generado en: {ruta}", "PDF creado", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar PDF: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
