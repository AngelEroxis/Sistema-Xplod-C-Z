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
using System.Data.Entity;
using WPF_LoginForm.Model;
namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para InventarioView.xaml
    /// </summary>
    public partial class InventarioView : UserControl
    {
        private List<Producto> productos;

        public InventarioView()
        {
            InitializeComponent();
            CargarInventario();
            dgInventario.SelectionChanged += DgInventario_SelectionChanged;
        }

        private void CargarInventario(string filtro = "")
        {
            using (var ctx = new MyDbContext())
            {
                var query = ctx.Productos
                                .Include(p => p.Proveedor)
                                .Include(p => p.Inventario)
                                .AsQueryable();

                if (!string.IsNullOrWhiteSpace(filtro))
                    query = query.Where(p => p.Nombre.Contains(filtro));

                productos = query.ToList();
                dgInventario.ItemsSource = productos;
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
            => CargarInventario(txtBuscar.Text);

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtBuscar.Clear();
            CargarInventario();
        }

        private void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            var win = new ProductoWindow(); // Ventana de alta/edición
            if (win.ShowDialog() == true)
                CargarInventario();
        }

        private Producto seleccionado => dgInventario.SelectedItem as Producto;

        private void DgInventario_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            // Habilitar/Deshabilitar según selección…
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Producto;
            if (prod != null)
            {
                var win = new ProductoWindow(prod);
                if (win.ShowDialog() == true)
                    CargarInventario(txtBuscar.Text);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var prod = (sender as Button).DataContext as Producto;
            if (prod == null) return;
            if (MessageBox.Show($"Eliminar \"{prod.Nombre}\"?", "Confirmar",
                               MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var ctx = new MyDbContext())
                {
                    var p = ctx.Productos.Find(prod.IdProducto);
                    if (p != null)
                    {
                        ctx.Inventarios.Remove(ctx.Inventarios.FirstOrDefault(i => i.IdProducto == p.IdProducto));
                        ctx.Productos.Remove(p);
                        ctx.SaveChanges();
                    }
                }
                CargarInventario(txtBuscar.Text);
            }
        }
        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            CargarInventario(txtBuscar.Text);
        }

    }
}
