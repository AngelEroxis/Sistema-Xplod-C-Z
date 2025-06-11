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

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }
        // Aquí agregamos los manejadores (event handlers)
        private void GestionClientes_Click(object sender, RoutedEventArgs e)
        {
            var clientesView = new ClientesView();
            // Suponiendo que el Grid principal de contenido tiene nombre "ContentGrid"
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(clientesView);
        }

        private void GestionProductos_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new InventarioView());
        }

        private void GestionVentas_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new VentasView());

        }

        private void GestionReportes_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Abrir Generación de Reportes");
        }

        private void GestionCompras_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new ComprasView());
        }

        private void GestionUsuarios_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Abrir Gestión de Usuarios");
            var ventanaUsuarios = new UserManagementView();
            ventanaUsuarios.ShowDialog(); // o .Show() si prefieres
        }

        private void CerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            // Mostrar un mensaje si es necesario
            MessageBox.Show("Sesión cerrada");

            // Abrir la ventana de login
            LoginView loginWindow = new LoginView();
            loginWindow.Show();

            // Cerrar la ventana actual
            this.Close();
        }
    }
}