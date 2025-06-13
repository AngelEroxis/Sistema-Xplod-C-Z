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

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para VendedorManagementView.xaml
    /// </summary>
    public partial class VendedorManagementView : Window
    {
        private ObservableCollection<Vendedor> vendedores = new ObservableCollection<Vendedor>();
        private Vendedor vendedorSeleccionado;
        public VendedorManagementView()
        {
            InitializeComponent();
            dgVendedores.ItemsSource = vendedores;
            CargarVendedores();
        }

        private void CargarVendedores()
        {
            using (var context = new MyDbContext())
            {
                vendedores.Clear();
                foreach (var v in context.Vendedores.ToList())
                {
                    vendedores.Add(v);
                }
            }
        }

        private void BtnAgregarEditar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string celular = txtCelular.Text.Trim();

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("El nombre es obligatorio.", "Campos incompletos", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new MyDbContext())
            {
                if (vendedorSeleccionado == null)
                {
                    // Agregar nuevo vendedor
                    var nuevoVendedor = new Vendedor { Nombre = nombre, Celular = celular };
                    context.Vendedores.Add(nuevoVendedor);
                }
                else
                {
                    // Editar vendedor existente
                    var vendedorDb = context.Vendedores.Find(vendedorSeleccionado.IdVendedor);
                    if (vendedorDb != null)
                    {
                        vendedorDb.Nombre = nombre;
                        vendedorDb.Celular = celular;
                    }
                }
                context.SaveChanges();
            }

            LimpiarFormulario();
            CargarVendedores();
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Vendedor vendedor)
            {
                var resultado = MessageBox.Show($"¿Seguro que deseas eliminar al vendedor '{vendedor.Nombre}'?", "Confirmar eliminación", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (resultado == MessageBoxResult.Yes)
                {
                    using (var context = new MyDbContext())
                    {
                        var vendedorDb = context.Vendedores.Find(vendedor.IdVendedor);
                        if (vendedorDb != null)
                        {
                            context.Vendedores.Remove(vendedorDb);
                            context.SaveChanges();
                        }
                    }
                    CargarVendedores();
                    LimpiarFormulario();
                }
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is Vendedor vendedor)
            {
                vendedorSeleccionado = vendedor;
                txtNombre.Text = vendedor.Nombre;
                txtCelular.Text = vendedor.Celular;

                btnAgregarEditar.Content = "💾 Guardar";
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Clear();
            txtCelular.Clear();
            vendedorSeleccionado = null;
            btnAgregarEditar.Content = "➕ Agregar";
            dgVendedores.SelectedIndex = -1;
        }

        private void DgVendedores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgVendedores.SelectedItem is Vendedor vendedor)
            {
                vendedorSeleccionado = vendedor;
                txtNombre.Text = vendedor.Nombre;
                txtCelular.Text = vendedor.Celular;
                btnAgregarEditar.Content = "💾 Guardar";
            }
        }
    }
}
