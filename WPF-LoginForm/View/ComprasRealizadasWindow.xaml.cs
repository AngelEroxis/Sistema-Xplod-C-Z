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
                var ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nombreArchivo);

                using (var fs = new FileStream(ruta, FileMode.Create))
                using (var doc = new Document(PageSize.A4, 25, 25, 30, 30))
                {
                    PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    // Encabezado
                    var encabezadoTabla = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    encabezadoTabla.SetWidths(new float[] { 50, 50 });

                    encabezadoTabla.AddCell(new PdfPCell(new Phrase("Compra de Productos para Xplod C&Z", FontFactory.GetFont("Arial", 12, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    });

                    encabezadoTabla.AddCell(new PdfPCell(new Phrase(compra.Fecha.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", 12, Font.NORMAL)))
                    {
                        HorizontalAlignment = Element.ALIGN_RIGHT,
                        Border = Rectangle.NO_BORDER
                    });

                    doc.Add(encabezadoTabla);
                    doc.Add(new Paragraph("\n"));

                    // Detalles de la compra
                    var detallesCompra = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };
                    detallesCompra.SetWidths(new float[] { 50, 50 });

                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Nro. Compra: {compra.IdCompra}", FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    });

                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Proveedor: {compra.Proveedor.Nombre}", FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    });

                    detallesCompra.AddCell(new PdfPCell(new Phrase($"Contacto: {compra.Proveedor.Contacto}", FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    });

                    /*detallesCompra.AddCell(new PdfPCell(new Phrase($"Vendedor: {compra.Vendedor.Nombre}", FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                    {
                        HorizontalAlignment = Element.ALIGN_LEFT,
                        Border = Rectangle.NO_BORDER
                    });*/

                    doc.Add(detallesCompra);
                    doc.Add(new Paragraph("\n"));

                    // Tabla de productos
                    var tablaProductos = new PdfPTable(6)
                    {
                        WidthPercentage = 100
                    };
                    tablaProductos.SetWidths(new float[] { 10, 30, 20, 10, 15, 15 });

                    tablaProductos.AddCell(new PdfPCell(new Phrase("Nro", FontFactory.GetFont("Arial", 10, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    });
                    tablaProductos.AddCell(new PdfPCell(new Phrase("Producto", FontFactory.GetFont("Arial", 10, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    });
                    tablaProductos.AddCell(new PdfPCell(new Phrase("Unidad Medida", FontFactory.GetFont("Arial", 10, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    });
                    tablaProductos.AddCell(new PdfPCell(new Phrase("Cantidad", FontFactory.GetFont("Arial", 10, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    });
                    tablaProductos.AddCell(new PdfPCell(new Phrase("Precio Unitario", FontFactory.GetFont("Arial", 10, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    });
                    tablaProductos.AddCell(new PdfPCell(new Phrase("Total", FontFactory.GetFont("Arial", 10, Font.BOLD)))
                    {
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        BackgroundColor = BaseColor.LIGHT_GRAY
                    });

                    decimal totalCompra = 0;
                    int nro = 1;
                    foreach (var detalle in compra.DetalleCompras)
                    {
                        tablaProductos.AddCell(new PdfPCell(new Phrase(nro.ToString(), FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Producto.Nombre, FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_LEFT
                        });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Producto.UnidadMedida, FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Cantidad.ToString(), FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });
                        tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.SubTotal.ToString("N2"), FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });
                        tablaProductos.AddCell(new PdfPCell(new Phrase((detalle.SubTotal * detalle.Cantidad).ToString("N2"), FontFactory.GetFont("Arial", 10, Font.NORMAL)))
                        {
                            HorizontalAlignment = Element.ALIGN_RIGHT
                        });

                        totalCompra += detalle.SubTotal * detalle.Cantidad;
                        nro++;
                    }

                    doc.Add(tablaProductos);
                    doc.Add(new Paragraph("\n"));

                    // Total de la compra
                    var totalCompraParrafo = new Paragraph($"Total de la compra: {totalCompra:N2}", FontFactory.GetFont("Arial", 12, Font.BOLD))
                    {
                        Alignment = Element.ALIGN_RIGHT
                    };
                    doc.Add(totalCompraParrafo);

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
