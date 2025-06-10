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
using System.Collections.ObjectModel;
using WPF_LoginForm.Model;
using System.Data.Entity;
using System.Diagnostics;



namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : Window
    {
        private ObservableCollection<Usuario> usuarios = new ObservableCollection<Usuario>();
        private Usuario usuarioSeleccionado;

        public UserManagementView()
        {
            InitializeComponent();
            dgUsuarios.ItemsSource = usuarios;
            CargarUsuarios();
            CargarVendedores();
        }

        private void CargarUsuarios()
        {

            using (var context = new MyDbContext())
            {
                usuarios.Clear();
                foreach (var usuario in context.Usuarios.Include(u => u.Vendedor).ToList())
                {
                    usuarios.Add(usuario);
                    //MessageBox.Show("Usuarios encontrados: " + usuarios.Count);

                }
                dgUsuarios.ItemsSource = usuarios;

            }
            using (var context = new MyDbContext())
            {
                var total = context.Usuarios.Count();
                //MessageBox.Show("Usuarios en la base: " + total);
            }

        }

        private List<Vendedor> listaVendedores;

        private void CargarVendedores()
        {
            try
            {
                using (var context = new MyDbContext())
                {
                    listaVendedores = context.Vendedores.ToList();
                    cmbVendedor.ItemsSource = listaVendedores;
                    cmbVendedor.DisplayMemberPath = "Nombre";
                    cmbVendedor.SelectedValuePath = "IdVendedor";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar vendedores: " + ex.Message);
            }
        }



        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string nombreUsuario = txtNombreUsuario.Text.Trim();
            string password = txtContrasena.Password.Trim();
            string rol = (cmbRol.SelectedItem as ComboBoxItem)?.Content?.ToString();
            //string idVendedor = txtIdVendedor.Text.Trim();
            int? idVendedor = cmbVendedor.SelectedValue as int?;

            if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(rol) || !idVendedor.HasValue)
            {
                MessageBox.Show("Por favor complete todos los campos.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            using (var context = new MyDbContext())
            {

                var nuevoUsuario = new Usuario
                {
                    NombreUsuario = nombreUsuario,
                    Contrasena = password,
                    Rol = rol,
                    IdVendedor = idVendedor
                };


                context.Usuarios.Add(nuevoUsuario);
                context.SaveChanges();
            }

            LimpiarFormulario();
            CargarUsuarios();
            MessageBox.Show("Usuario agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsuarios.SelectedItem is Usuario usuario)
            {
                var resultado = MessageBox.Show($"¿Seguro que deseas eliminar al usuario '{usuario.NombreUsuario}'?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    using (var context = new MyDbContext())
                    {
                        var usuarioEliminar = context.Usuarios.Find(usuario.IdUsuario);
                        if (usuarioEliminar != null)
                        {
                            context.Usuarios.Remove(usuarioEliminar);
                            context.SaveChanges();
                            CargarUsuarios();
                        }
                    }
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsuarios.SelectedItem is Usuario usuario)
            {
                usuarioSeleccionado = usuario;
                txtNombreUsuario.Text = usuario.NombreUsuario;
                txtContrasena.Password = ""; // No se puede mostrar el hash
                cmbRol.SelectedItem = cmbRol.Items.Cast<ComboBoxItem>().FirstOrDefault(i => i.Content.ToString() == usuario.Rol);

                btnAgregar.Content = "💾 Guardar";
                btnAgregar.Click -= BtnAgregar_Click;
                btnAgregar.Click += BtnGuardarCambios_Click;
            }
        }

        private void BtnGuardarCambios_Click(object sender, RoutedEventArgs e)
        {
            if (usuarioSeleccionado == null) return;

            string nuevoNombre = txtNombreUsuario.Text.Trim();
            string nuevaContrasena = txtContrasena.Password.Trim();
            string nuevoRol = (cmbRol.SelectedItem as ComboBoxItem)?.Content?.ToString();
            int? nuevoIdVendedor = cmbVendedor.SelectedValue as int?;

            using (var context = new MyDbContext())
            {
                var usuarioDb = context.Usuarios.Find(usuarioSeleccionado.IdUsuario);
                if (usuarioDb != null)
                {
                    usuarioDb.NombreUsuario = nuevoNombre;
                    usuarioDb.Rol = nuevoRol;

                    if (nuevoIdVendedor.HasValue)
                        usuarioDb.IdVendedor = nuevoIdVendedor;

                    if (!string.IsNullOrWhiteSpace(nuevaContrasena))
                        usuarioDb.Contrasena = nuevaContrasena;

                    context.SaveChanges();
                }
            }

            LimpiarFormulario();
            CargarUsuarios();

            btnAgregar.Content = "➕ Agregar";
            btnAgregar.Click -= BtnGuardarCambios_Click;
            btnAgregar.Click += BtnAgregar_Click;

            usuarioSeleccionado = null;
        }


        private void LimpiarFormulario()
        {
            txtNombreUsuario.Clear();
            txtContrasena.Clear();
            cmbRol.SelectedIndex = -1;
            cmbVendedor.SelectedIndex = -1;

        }
    }
}
