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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_LoginForm.Model;

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para GestionClientesView.xaml
    /// </summary>
    public partial class GestionClientesView : UserControl
    {
        public GestionClientesView()
        {
            InitializeComponent();
            CargarClientes();
        }
        private void CargarClientes()
        {
            using (var context = new MyDbContext())
            {
                var clientes = context.Clientes
                    .Include("Creditos")
                    .ToList();
                dgClientes.ItemsSource = clientes;
            }
        }

        // 🟢 Método que se llama cuando seleccionas un cliente de la lista
        private void dgClientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgClientes.SelectedItem is Cliente cliente)
            {
                // Aquí podrías asignarlo a un control o ViewModel, según tu arquitectura
                MessageBox.Show($"Cliente seleccionado: {cliente.Nombre}");
            }
        }

        // 🟢 Método que se llama al hacer clic en "Ver pagos"
        private void VerPagos_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is Credito credito)
            {
                MessageBox.Show($"Pagos del crédito #{credito.IdCredito} - Cliente #{credito.IdCliente}");

                // Aquí puedes abrir una nueva ventana o UserControl con los pagos
                // Por ahora solo muestra un mensaje
            }
        }
    }
}
