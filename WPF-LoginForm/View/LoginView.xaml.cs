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
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUser.Text; // asegúrate de que el TextBox tenga x:Name="txtUsername"
            string password = txtPass.Password; // asegúrate de que el PasswordBox tenga x:Name="txtPassword"

            var user = AuthService.Login(username, password);

            if (user != null)
            {
                SesionActual.UsuarioLogueado = AuthService.Login(username, password);

                if (user.Rol == "Administrador")
                {
                    var adminWindow = new AdminDashboard();
                    adminWindow.Show();
                }
                else if (user.Rol == "Vendedor")
                {
                    var vendedorWindow = new VendedorDashboard();
                    vendedorWindow.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales incorrectas", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
