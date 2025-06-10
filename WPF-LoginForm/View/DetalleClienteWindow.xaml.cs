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
}
