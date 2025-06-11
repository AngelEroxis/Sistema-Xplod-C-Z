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
    /// Lógica de interacción para ProveedorWindow.xaml
    /// </summary>
    public partial class ProveedorWindow : Window
    {
        private Proveedor proveedorSeleccionado = null;

        public ProveedorWindow()
        {
            InitializeComponent();
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            using (var ctx = new MyDbContext())
            {
                var lista = ctx.Proveedores.ToList();
                dgProveedores.ItemsSource = lista;
            }
        }

        private void dgProveedores_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            proveedorSeleccionado = dgProveedores.SelectedItem as Proveedor;
            if (proveedorSeleccionado != null)
            {
                txtNombre.Text = proveedorSeleccionado.Nombre;
                txtContacto.Text = proveedorSeleccionado.Contacto;
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string contacto = txtContacto.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre))
            {
                MessageBox.Show("El nombre del proveedor es obligatorio.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var ctx = new MyDbContext())
            {
                if (proveedorSeleccionado == null)
                {
                    // Agregar nuevo proveedor
                    var nuevoProveedor = new Proveedor
                    {
                        Nombre = nombre,
                        Contacto = string.IsNullOrWhiteSpace(contacto) ? null : contacto
                    };

                    ctx.Proveedores.Add(nuevoProveedor);
                }
                else
                {
                    // Editar proveedor existente
                    var proveedor = ctx.Proveedores.FirstOrDefault(p => p.IdProveedor == proveedorSeleccionado.IdProveedor);
                    if (proveedor != null)
                    {
                        proveedor.Nombre = nombre;
                        proveedor.Contacto = string.IsNullOrWhiteSpace(contacto) ? null : contacto;
                    }
                }

                ctx.SaveChanges();
            }

            MessageBox.Show("Proveedor guardado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            proveedorSeleccionado = null;
            txtNombre.Clear();
            txtContacto.Clear();
            CargarProveedores();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
