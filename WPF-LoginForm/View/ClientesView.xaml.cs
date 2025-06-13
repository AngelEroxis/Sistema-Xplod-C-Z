using System;
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
using System.Collections.Generic;
using WPF_LoginForm.Model;
using System.Data.Entity;



namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para ClientesView.xaml
    /// </summary>
    public partial class ClientesView : UserControl
    {
        public ClientesView()
        {
            InitializeComponent();
            CargarDatosClientes();
        }

        private void CargarDatosClientes()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    var clientes = context.Clientes
                        .Include(c => c.Creditos)
                        .ToList();

                    dgClientes.ItemsSource = clientes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos de clientes: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedCliente = dgClientes.SelectedItem as Cliente;

            if (selectedCliente != null)
            {
                btnEditarCliente.IsEnabled = true;
                btnEliminarCliente.IsEnabled = true;
                btnVerDetalles.IsEnabled = true;
                btnAsignarCredito.IsEnabled = true;

            }
            else
            {
                btnEditarCliente.IsEnabled = false;
                btnEliminarCliente.IsEnabled = false;
                btnVerDetalles.IsEnabled = false; 
                btnAsignarCredito.IsEnabled = false;
            }
        }

        private void BtnAgregarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var registrarWindow = new RegistrarClienteWindow();
                if (registrarWindow.ShowDialog() == true)
                {
                    CargarDatosClientes();
                    MessageBox.Show("Cliente agregado correctamente.", "Éxito",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar cliente: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEditarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clienteSeleccionado = dgClientes.SelectedItem as Cliente;
                if (clienteSeleccionado != null)
                {
                    var ventanaEditar = new RegistrarClienteWindow(clienteSeleccionado);
                    if (ventanaEditar.ShowDialog() == true)
                    {
                        CargarDatosClientes();
                        MessageBox.Show("Cliente editado correctamente.", "Éxito",
                                       MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar cliente: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEliminarCliente_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clienteSeleccionado = dgClientes.SelectedItem as Cliente;
                if (clienteSeleccionado != null)
                {
                    // Verificar si el cliente tiene créditos activos
                    using (var context = new MyDbContext())
                    {
                        var tieneCreditos = context.Creditos
                            .Any(c => c.IdCliente == clienteSeleccionado.IdCliente &&
                                     c.SaldoPendiente > 0);

                        if (tieneCreditos)
                        {
                            MessageBox.Show("No se puede eliminar este cliente porque tiene créditos pendientes de pago.",
                                           "No se puede eliminar", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    var resultado = MessageBox.Show($"¿Estás seguro de que deseas eliminar al cliente '{clienteSeleccionado.Nombre}'?",
                                                   "Confirmar Eliminación",
                                                   MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (resultado == MessageBoxResult.Yes)
                    {
                        using (var context = new MyDbContext())
                        {
                            var clienteDb = context.Clientes.FirstOrDefault(c => c.IdCliente == clienteSeleccionado.IdCliente);
                            if (clienteDb != null)
                            {
                                context.Clientes.Remove(clienteDb);
                                context.SaveChanges();
                                MessageBox.Show("Cliente eliminado correctamente", "Éxito",
                                               MessageBoxButton.OK, MessageBoxImage.Information);
                                CargarDatosClientes();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar cliente: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // NUEVA FUNCIONALIDAD: Abrir ventana de detalles de créditos
        private void BtnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var clienteSeleccionado = dgClientes.SelectedItem as Cliente;
                if (clienteSeleccionado != null)
                {
                    // Cargar el cliente con todos sus datos relacionados desde la base de datos
                    using (var context = new MyDbContext())
                    {
                        var clienteCompleto = context.Clientes
                            .Include(c => c.Creditos)
                            .FirstOrDefault(c => c.IdCliente == clienteSeleccionado.IdCliente);

                        if (clienteCompleto != null)
                        {
                            // Abrir la ventana de detalles
                            var ventanaDetalles = new DetalleClienteWindow(clienteCompleto);
                            ventanaDetalles.ShowDialog();

                            // Refrescar los datos después de cerrar la ventana de detalles
                            // (por si se realizaron pagos o cambios)
                            CargarDatosClientes();
                        }
                        else
                        {
                            MessageBox.Show("No se pudo cargar la información del cliente.",
                                           "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir los detalles del cliente: {ex.Message}",
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void BtnAsignarCredito_Click(object sender, RoutedEventArgs e)
        {
            var clienteSeleccionado = dgClientes.SelectedItem as Cliente;

            if (clienteSeleccionado != null)
            {
                var ventanaAsignar = new AsignarCreditoWindow(clienteSeleccionado);
                if (ventanaAsignar.ShowDialog() == true)
                {
                    CargarDatosClientes(); // Refresca datos con nuevo crédito
                    MessageBox.Show("Crédito asignado correctamente.", "Éxito",
                                   MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

    }
}
