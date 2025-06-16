// Primero, necesitas instalar el paquete NuGet iTextSharp
// En Package Manager Console ejecuta: Install-Package iTextSharp

using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.Entity;
using WPF_LoginForm.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.Win32;
using iTextSharpText = iTextSharp.text;
using iTextSharpPdf = iTextSharp.text.pdf;

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para DetalleClienteWindow.xaml
    /// </summary>
    public partial class DetalleClienteWindow : Window
    {
        private Cliente _clienteSeleccionado;
        private List<Credito> _creditosCliente;

        public DetalleClienteWindow(Cliente cliente)
        {
            InitializeComponent();
            _clienteSeleccionado = cliente;
            CargarDatosCliente();
        }

        // ... (mantener todos los métodos existentes)

        private void CargarDatosCliente()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    // Cargar cliente con sus créditos
                    _clienteSeleccionado = context.Clientes
                        .Include(c => c.Creditos)
                        .FirstOrDefault(c => c.IdCliente == _clienteSeleccionado.IdCliente);

                    if (_clienteSeleccionado != null)
                    {
                        // Actualizar información del header
                        txtNombreCliente.Text = _clienteSeleccionado.Nombre;
                        txtInfoCliente.Text = $"CI: {_clienteSeleccionado.CI} | Celular: {_clienteSeleccionado.Celular} | Domicilio: {_clienteSeleccionado.Domicilio}";

                        // Cargar créditos
                        _creditosCliente = _clienteSeleccionado.Creditos?.ToList() ?? new List<Credito>();
                        dgCreditos.ItemsSource = _creditosCliente;

                        // Actualizar resumen
                        ActualizarResumen();

                        // Cargar historial de pagos
                        CargarHistorialPagos();

                        // Cargar productos comprados
                        CargarProductosComprados();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del cliente: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Método para generar PDF del cliente
        private void GenerarPDFCliente()
        {
            try
            {
                // Abrir diálogo para guardar el archivo
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"Contrato_Cliente_{_clienteSeleccionado.Nombre.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd}.pdf"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Crear el documento PDF
                    Document document = new Document(PageSize.A4, 50, 50, 25, 25);
                    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(saveFileDialog.FileName, FileMode.Create));

                    document.Open();

                    // Configurar fuentes
                    BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    Font titleFont = new Font(baseFont, 18, Font.BOLD);
                    Font headerFont = new Font(baseFont, 14, Font.BOLD);
                    Font normalFont = new Font(baseFont, 10, Font.NORMAL);
                    Font boldFont = new Font(baseFont, 10, Font.BOLD);

                    // Título del documento
                    iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("CONTRATO DE CRÉDITO", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 20;
                    document.Add(title);

                    // Información del cliente
                    document.Add(new iTextSharp.text.Paragraph("DATOS DEL CLIENTE", headerFont));
                    document.Add(new iTextSharp.text.Paragraph($"Número: {_clienteSeleccionado.IdCliente}", normalFont));
                    document.Add(new iTextSharp.text.Paragraph($"Nombre: {_clienteSeleccionado.Nombre}", normalFont));
                    document.Add(new iTextSharp.text.Paragraph($"Empresa o Institución: {_clienteSeleccionado.EmpresaInstitucion ?? "N/A"}", normalFont));
                    document.Add(new iTextSharp.text.Paragraph($"Fecha: {DateTime.Now:dd/MM/yyyy}", normalFont));
                    document.Add(new iTextSharp.text.Paragraph($"Celular: {_clienteSeleccionado.Celular}", normalFont));
                    document.Add(new iTextSharp.text.Paragraph($"Domicilio: {_clienteSeleccionado.Domicilio}", normalFont));
                    document.Add(new iTextSharp.text.Paragraph($"C.I.: {_clienteSeleccionado.CI}", normalFont));

                    // Información del garante (si existe)
                    if (!string.IsNullOrEmpty(_clienteSeleccionado.Garante))
                    {
                        document.Add(new iTextSharp.text.Paragraph($"Garante: {_clienteSeleccionado.Garante}", normalFont));
                        document.Add(new iTextSharp.text.Paragraph($"Celular Garante: {_clienteSeleccionado.CelGarante ?? "N/A"}", normalFont));
                    }

                    document.Add(new iTextSharp.text.Paragraph(" ", normalFont)); // Espacio

                    // Obtener productos comprados
                    var productosComprados = ObtenerProductosComprados();

                    // Crear tabla de productos
                    document.Add(new iTextSharp.text.Paragraph("DETALLE DE PRODUCTOS ADQUIRIDOS", headerFont));

                    PdfPTable tableProductos = new PdfPTable(5);
                    tableProductos.WidthPercentage = 100;
                    tableProductos.SetWidths(new float[] { 1f, 3f, 1f, 2f, 2f });

                    // Headers de la tabla
                    tableProductos.AddCell(new PdfPCell(new Phrase("Item", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tableProductos.AddCell(new PdfPCell(new Phrase("Producto", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tableProductos.AddCell(new PdfPCell(new Phrase("Cant.", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tableProductos.AddCell(new PdfPCell(new Phrase("Precio Unit.", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tableProductos.AddCell(new PdfPCell(new Phrase("Subtotal", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                    int itemNumber = 1;
                    decimal totalGeneral = 0;

                    foreach (var producto in productosComprados)
                    {
                        tableProductos.AddCell(new PdfPCell(new Phrase(itemNumber.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tableProductos.AddCell(new PdfPCell(new Phrase(producto.NombreProducto, normalFont)));
                        tableProductos.AddCell(new PdfPCell(new Phrase(producto.Cantidad.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                        tableProductos.AddCell(new PdfPCell(new Phrase($"Bs. {producto.PrecioUnitario:F2}", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                        tableProductos.AddCell(new PdfPCell(new Phrase($"Bs. {producto.Subtotal:F2}", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                        totalGeneral += producto.Subtotal;
                        itemNumber++;
                    }

                    // Fila del total
                    tableProductos.AddCell(new PdfPCell(new Phrase("", normalFont)) { Border = 0 });
                    tableProductos.AddCell(new PdfPCell(new Phrase("", normalFont)) { Border = 0 });
                    tableProductos.AddCell(new PdfPCell(new Phrase("", normalFont)) { Border = 0 });
                    tableProductos.AddCell(new PdfPCell(new Phrase("TOTAL:", boldFont)) { HorizontalAlignment = Element.ALIGN_RIGHT, Border = 0 });
                    tableProductos.AddCell(new PdfPCell(new Phrase($"Bs. {totalGeneral:F2}", boldFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });

                    document.Add(tableProductos);
                    document.Add(new iTextSharp.text.Paragraph(" ", normalFont)); // Espacio

                    // Información de crédito
                    if (_creditosCliente != null && _creditosCliente.Any())
                    {
                        document.Add(new iTextSharp.text.Paragraph("INFORMACIÓN DEL CRÉDITO", headerFont));

                        foreach (var credito in _creditosCliente)
                        {
                            document.Add(new iTextSharp.text.Paragraph($"Monto Total del Crédito: Bs. {credito.MontoTotal:F2}", normalFont));
                            document.Add(new iTextSharp.text.Paragraph($"Número de Cuotas: {credito.Cuotas}", normalFont));
                            document.Add(new iTextSharp.text.Paragraph($"Monto de Cuota Mensual: Bs. {credito.CuotaMensual:F2}", normalFont));
                            document.Add(new iTextSharp.text.Paragraph(" ", normalFont));

                            // Tabla de cuotas
                            document.Add(new iTextSharp.text.Paragraph("CRONOGRAMA DE PAGOS", headerFont));

                            PdfPTable tableCuotas = new PdfPTable(3);
                            tableCuotas.WidthPercentage = 100;
                            tableCuotas.SetWidths(new float[] { 1f, 2f, 2f });

                            // Headers
                            tableCuotas.AddCell(new PdfPCell(new Phrase("Cuota #", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            tableCuotas.AddCell(new PdfPCell(new Phrase("Fecha Vencimiento", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                            tableCuotas.AddCell(new PdfPCell(new Phrase("Monto", boldFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                            var cuotas = GenerarCuotas(credito);
                            foreach (var cuota in cuotas)
                            {
                                tableCuotas.AddCell(new PdfPCell(new Phrase(cuota.NumeroCuota.ToString(), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                tableCuotas.AddCell(new PdfPCell(new Phrase(cuota.FechaVencimiento.ToString("dd/MM/yyyy"), normalFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                                tableCuotas.AddCell(new PdfPCell(new Phrase($"Bs. {cuota.MontoCuota:F2}", normalFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                            }

                            document.Add(tableCuotas);
                            document.Add(new iTextSharp.text.Paragraph(" ", normalFont));
                        }
                    }

                    // Espacio para firma
                    document.Add(new iTextSharp.text.Paragraph(" ", normalFont));
                    document.Add(new iTextSharp.text.Paragraph(" ", normalFont));
                    document.Add(new iTextSharp.text.Paragraph(" ", normalFont));

                    // Sección de firmas
                    PdfPTable tableFirmas = new PdfPTable(2);
                    tableFirmas.WidthPercentage = 100;
                    tableFirmas.SetWidths(new float[] { 1f, 1f });

                    PdfPCell firmaCliente = new PdfPCell();
                    firmaCliente.Border = 0;
                    firmaCliente.AddElement(new iTextSharp.text.Paragraph("_________________________", normalFont));
                    firmaCliente.AddElement(new iTextSharp.text.Paragraph("Firma del Cliente", normalFont));
                    firmaCliente.AddElement(new iTextSharp.text.Paragraph($"C.I.: {_clienteSeleccionado.CI}", normalFont));
                    firmaCliente.HorizontalAlignment = Element.ALIGN_CENTER;

                    PdfPCell firmaEmpresa = new PdfPCell();
                    firmaEmpresa.Border = 0;
                    firmaEmpresa.AddElement(new iTextSharp.text.Paragraph("_________________________", normalFont));
                    firmaEmpresa.AddElement(new iTextSharp.text.Paragraph("Representante Legal", normalFont));
                    firmaEmpresa.AddElement(new iTextSharp.text.Paragraph("Sistema Xplod C-Z", normalFont));
                    firmaEmpresa.HorizontalAlignment = Element.ALIGN_CENTER;

                    tableFirmas.AddCell(firmaCliente);
                    tableFirmas.AddCell(firmaEmpresa);

                    document.Add(tableFirmas);

                    document.Close();

                    MessageBox.Show($"PDF generado exitosamente en: {saveFileDialog.FileName}", "Éxito",
                                   MessageBoxButton.OK, MessageBoxImage.Information);

                    // Abrir el PDF generado
                    System.Diagnostics.Process.Start(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el PDF: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<DetalleProductoVenta> ObtenerProductosComprados()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var productosComprados = (from v in context.Ventas
                                              join dv in context.DetalleVentas on v.IdVenta equals dv.IdVenta
                                              join p in context.Productos on dv.IdProducto equals p.IdProducto
                                              where v.IdCliente == _clienteSeleccionado.IdCliente
                                              select new DetalleProductoVenta
                                              {
                                                  FechaVenta = v.Fecha,
                                                  NombreProducto = p.Nombre,
                                                  DescripcionProducto = p.Descripcion,
                                                  Cantidad = dv.Cantidad,
                                                  PrecioUnitario = dv.PrecioUnitario,
                                                  Subtotal = dv.Subtotal,
                                                  TotalVenta = v.Total,
                                                  IdVenta = v.IdVenta
                                              })
                                            .OrderByDescending(x => x.FechaVenta)
                                            .ToList();

                    return productosComprados;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al obtener productos comprados: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<DetalleProductoVenta>();
            }
        }

        // Event handler para el botón de generar PDF
        private void BtnGenerarPDF_Click(object sender, RoutedEventArgs e)
        {
            GenerarPDFCliente();
        }

        // ... (mantener todos los demás métodos existentes)

        private void ActualizarResumen()
        {
            if (_creditosCliente != null && _creditosCliente.Any())
            {
                var montoTotal = _creditosCliente.Sum(c => c.MontoTotal);
                var saldoPendiente = _creditosCliente.Sum(c => c.SaldoPendiente);
                var totalCuotas = _creditosCliente.Sum(c => c.Cuotas);
                var estado = saldoPendiente > 0 ? "PENDIENTE" : "PAGADO";

                txtMontoTotal.Text = $"Bs. {montoTotal:F2}";
                txtSaldoPendiente.Text = $"Bs. {saldoPendiente:F2}";
                txtTotalCuotas.Text = totalCuotas.ToString();
                txtEstado.Text = estado;

                // Cambiar color según el estado
                if (saldoPendiente > 0)
                {
                    txtEstado.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(220, 53, 69)); // #dc3545
                }
                else
                {
                    txtEstado.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(40, 167, 69)); // #28a745
                }
            }
            else
            {
                txtMontoTotal.Text = "Bs. 0.00";
                txtSaldoPendiente.Text = "Bs. 0.00";
                txtTotalCuotas.Text = "0";
                txtEstado.Text = "SIN CRÉDITO";
                txtEstado.Foreground = new System.Windows.Media.SolidColorBrush(
                    System.Windows.Media.Color.FromRgb(108, 117, 125)); // #6c757d
            }
        }

        private void dgCreditos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var creditoSeleccionado = dgCreditos.SelectedItem as Credito;
            if (creditoSeleccionado != null)
            {
                CargarCuotasCredito(creditoSeleccionado);
            }
            else
            {
                dgCuotas.ItemsSource = null;
            }
        }

        private void CargarCuotasCredito(Credito credito)
        {
            try
            {
                // Generar cuotas basadas en el crédito
                var cuotas = GenerarCuotas(credito);
                dgCuotas.ItemsSource = cuotas;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las cuotas: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<Cuota> GenerarCuotas(Credito credito)
        {
            var cuotas = new List<Cuota>();

            using (var context = new MyDbContext())
            {
                // Obtener la fecha de la venta para calcular las fechas de vencimiento
                var venta = context.Ventas.FirstOrDefault(v => v.IdVenta == credito.IdVenta);
                var fechaInicio = venta?.Fecha ?? DateTime.Now;

                // Obtener pagos realizados para este crédito
                var pagos = context.Pagos
                    .Where(p => p.IdVenta == credito.IdVenta)
                    .OrderBy(p => p.FechaPago)
                    .ToList();

                decimal montoPagado = 0;
                decimal cuotaMensual = credito.CuotaMensual;

                for (int i = 1; i <= credito.Cuotas; i++)
                {
                    var fechaVencimiento = fechaInicio.AddMonths(i);
                    var cuota = new Cuota
                    {
                        NumeroCuota = i,
                        FechaVencimiento = fechaVencimiento,
                        MontoCuota = cuotaMensual,
                        IdCredito = credito.IdCredito,
                        EstaPagada = false,
                        FechaPago = null,
                        EstadoCuota = "PENDIENTE"
                    };

                    // Verificar si esta cuota está pagada
                    if (montoPagado + cuotaMensual <= pagos.Sum(p => p.MontoPagado))
                    {
                        cuota.EstaPagada = true;
                        cuota.EstadoCuota = "PAGADA";

                        // Buscar la fecha de pago más cercana
                        var pagoAplicable = pagos.FirstOrDefault(p => p.MontoPagado >= cuotaMensual);
                        if (pagoAplicable != null)
                        {
                            cuota.FechaPago = pagoAplicable.FechaPago;
                        }
                        montoPagado += cuotaMensual;
                    }
                    else if (fechaVencimiento < DateTime.Now && !cuota.EstaPagada)
                    {
                        cuota.EstadoCuota = "VENCIDA";
                    }

                    cuotas.Add(cuota);
                }
            }

            return cuotas;
        }

        private void CargarHistorialPagos()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    // Obtener todos los pagos de las ventas del cliente
                    var ventasCliente = context.Ventas
                        .Where(v => v.IdCliente == _clienteSeleccionado.IdCliente)
                        .Select(v => v.IdVenta)
                        .ToList();

                    var pagos = context.Pagos
                        .Where(p => ventasCliente.Contains(p.IdVenta))
                        .OrderByDescending(p => p.FechaPago)
                        .ToList();

                    dgPagos.ItemsSource = pagos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el historial de pagos: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CargarProductosComprados()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var productosComprados = (from v in context.Ventas
                                              join dv in context.DetalleVentas on v.IdVenta equals dv.IdVenta
                                              join p in context.Productos on dv.IdProducto equals p.IdProducto
                                              where v.IdCliente == _clienteSeleccionado.IdCliente
                                              select new DetalleProductoVenta
                                              {
                                                  FechaVenta = v.Fecha,
                                                  NombreProducto = p.Nombre,
                                                  DescripcionProducto = p.Descripcion,
                                                  Cantidad = dv.Cantidad,
                                                  PrecioUnitario = dv.PrecioUnitario,
                                                  Subtotal = dv.Subtotal,
                                                  TotalVenta = v.Total,
                                                  IdVenta = v.IdVenta
                                              })
                                            .OrderByDescending(x => x.FechaVenta)
                                            .ToList();

                    dgProductos.ItemsSource = productosComprados;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los productos comprados: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnVolver_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnRegistrarPago_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Verificar si el cliente tiene saldo pendiente
                if (_creditosCliente == null || !_creditosCliente.Any() ||
                    _creditosCliente.Sum(c => c.SaldoPendiente) <= 0)
                {
                    MessageBox.Show("Este cliente no tiene saldo pendiente.", "Información",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var ventanaRegistrarPago = new RegistrarPagoWindow(_clienteSeleccionado, _creditosCliente);
                if (ventanaRegistrarPago.ShowDialog() == true)
                {
                    // Refrescar los datos después de registrar el pago
                    CargarDatosCliente();
                    MessageBox.Show("Pago registrado exitosamente.", "Éxito",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir la ventana de registro de pago: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Configurar el foco inicial
            dgCreditos.Focus();
        }
    }

    // Clase auxiliar para el detalle de productos en venta
    public class DetalleProductoVenta
    {
        public DateTime FechaVenta { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalVenta { get; set; }
        public int IdVenta { get; set; }
    }

    // Clase auxiliar para las cuotas
    public class Cuota
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCuota { get; set; }
        public int IdCredito { get; set; }
        public bool EstaPagada { get; set; }
        public DateTime? FechaPago { get; set; }
        public string EstadoCuota { get; set; }
    }
}