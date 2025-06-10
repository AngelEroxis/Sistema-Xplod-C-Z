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
using WPF_LoginForm.Model;

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para RegistrarPagoWindow.xaml
    /// </summary>
    public partial class RegistrarPagoWindow : Window
    {
        private Cliente _cliente;
        private List<Credito> _creditos;
        private Credito _creditoSeleccionado;

        public RegistrarPagoWindow(Cliente cliente, List<Credito> creditos)
        {
            InitializeComponent();
            _cliente = cliente;
            _creditos = creditos;
            InicializarVentana();
        }

        private void InicializarVentana()
        {
            // Mostrar información del cliente
            txtInfoCliente.Text = $"Cliente: {_cliente.Nombre} - CI: {_cliente.CI}";

            // Configurar fecha actual
            dpFechaPago.SelectedDate = DateTime.Now;

            // Cargar créditos con saldo pendiente
            CargarCreditos();
        }

        private void CargarCreditos()
        {
            // Crear lista de créditos con saldo pendiente
            var creditosConSaldo = _creditos
                .Where(c => c.SaldoPendiente > 0)
                .Select(c => new CreditoDisplay
                {
                    IdCredito = c.IdCredito,
                    IdVenta = c.IdVenta,
                    SaldoPendiente = c.SaldoPendiente,
                    DisplayText = $"Crédito #{c.IdCredito} - Venta #{c.IdVenta} - Saldo: Bs. {c.SaldoPendiente:F2}",
                    CreditoOriginal = c
                })
                .ToList();

            cbCreditos.ItemsSource = creditosConSaldo;

            if (creditosConSaldo.Any())
            {
                cbCreditos.SelectedIndex = 0;
            }
        }

        private void cbCreditos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var creditoDisplay = cbCreditos.SelectedItem as CreditoDisplay;
            if (creditoDisplay != null)
            {
                _creditoSeleccionado = creditoDisplay.CreditoOriginal;
                txtSaldoPendiente.Text = $"Bs. {_creditoSeleccionado.SaldoPendiente:F2}";

                // Limpiar monto de pago
                txtMontoPago.Text = "";
                txtNuevoSaldo.Text = $"Bs. {_creditoSeleccionado.SaldoPendiente:F2}";

                ValidarFormulario();
            }
        }

        private void txtMontoPago_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_creditoSeleccionado != null && decimal.TryParse(txtMontoPago.Text, out decimal montoPago))
            {
                var nuevoSaldo = Math.Max(0, _creditoSeleccionado.SaldoPendiente - montoPago);
                txtNuevoSaldo.Text = $"Bs. {nuevoSaldo:F2}";

                // Cambiar color según si se paga completo o parcial
                if (nuevoSaldo == 0)
                {
                    txtNuevoSaldo.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(40, 167, 69)); // Verde - Pagado completo
                }
                else
                {
                    txtNuevoSaldo.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(255, 193, 7)); // Amarillo - Pago parcial
                }
            }
            else
            {
                if (_creditoSeleccionado != null)
                {
                    txtNuevoSaldo.Text = $"Bs. {_creditoSeleccionado.SaldoPendiente:F2}";
                    txtNuevoSaldo.Foreground = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromRgb(220, 53, 69)); // Rojo - Sin cambios
                }
            }

            ValidarFormulario();
        }

        private void ValidarFormulario()
        {
            bool esValido = true;

            // Validar que haya un crédito seleccionado
            if (_creditoSeleccionado == null)
            {
                esValido = false;
            }

            // Validar monto
            if (!decimal.TryParse(txtMontoPago.Text, out decimal montoPago) || montoPago <= 0)
            {
                esValido = false;
            }
            else if (montoPago > _creditoSeleccionado.SaldoPendiente)
            {
                esValido = false;
                MessageBox.Show("El monto del pago no puede ser mayor al saldo pendiente.", "Error de Validación",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            // Validar fecha
            if (!dpFechaPago.SelectedDate.HasValue)
            {
                esValido = false;
            }

            // Validar método de pago
            if (cbMetodoPago.SelectedItem == null)
            {
                esValido = false;
            }

            btnGuardar.IsEnabled = esValido;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidarDatos())
                    return;

                var montoPago = decimal.Parse(txtMontoPago.Text);
                var fechaPago = dpFechaPago.SelectedDate.Value;
                var metodoPago = ((ComboBoxItem)cbMetodoPago.SelectedItem).Content.ToString();

                using (var context = new MyDbContext())
                {
                    // Crear nuevo registro de pago
                    var nuevoPago = new Pago
                    {
                        IdVenta = _creditoSeleccionado.IdVenta,
                        FechaPago = fechaPago,
                        MontoPagado = montoPago,
                        MetodoPago = metodoPago
                    };

                    context.Pagos.Add(nuevoPago);

                    // Actualizar saldo pendiente del crédito
                    var creditoDb = context.Creditos.FirstOrDefault(c => c.IdCredito == _creditoSeleccionado.IdCredito);
                    if (creditoDb != null)
                    {
                        creditoDb.SaldoPendiente -= montoPago;

                        // Si el saldo es 0, cambiar estado a "PAGADO"
                        if (creditoDb.SaldoPendiente == 0)
                        {
                            creditoDb.EstadoCredito = "PAGADO";
                        }
                        else
                        {
                            creditoDb.EstadoCredito = "PENDIENTE";
                        }
                    }

                    context.SaveChanges();

                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar el pago: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidarDatos()
        {
            if (_creditoSeleccionado == null)
            {
                MessageBox.Show("Debe seleccionar un crédito.", "Error de Validación",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(txtMontoPago.Text, out decimal montoPago) || montoPago <= 0)
            {
                MessageBox.Show("Debe ingresar un monto válido mayor a 0.", "Error de Validación",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMontoPago.Focus();
                return false;
            }

            if (montoPago > _creditoSeleccionado.SaldoPendiente)
            {
                MessageBox.Show("El monto del pago no puede ser mayor al saldo pendiente.", "Error de Validación",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMontoPago.Focus();
                return false;
            }

            if (!dpFechaPago.SelectedDate.HasValue)
            {
                MessageBox.Show("Debe seleccionar una fecha para el pago.", "Error de Validación",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (cbMetodoPago.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un método de pago.", "Error de Validación",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }

    // Clase auxiliar para mostrar créditos en el ComboBox
    public class CreditoDisplay
    {
        public int IdCredito { get; set; }
        public int IdVenta { get; set; }
        public decimal SaldoPendiente { get; set; }
        public string DisplayText { get; set; }
        public Credito CreditoOriginal { get; set; }
    }
}
