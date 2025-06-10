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
using System.Collections.ObjectModel;
using WPF_LoginForm.Model;
using System.Data.Entity; // Asegúrate de tener esto para que funcione Include con lambda

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para VentasView.xaml
    /// </summary>
    public partial class VentasView : UserControl
    {

        private List<CarritoItem> carrito = new List<CarritoItem>();
        private List<Producto> productos;

        public VentasView()
        {
            InitializeComponent();
            CargarProductos();
            dgCarrito.ItemsSource = carrito;
        }

        private void CargarProductos(string filtro = "")
        {
            using (var ctx = new MyDbContext())
            {
                var query = ctx.Productos.Include(p => p.Inventario);

                if (!string.IsNullOrWhiteSpace(filtro))
                    query = query.Where(p => p.Nombre.Contains(filtro));

                productos = query.ToList();
                dgProductosVenta.ItemsSource = productos;
            }
        }


        private void TxtBuscarProducto_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtPlaceholder.Visibility = string.IsNullOrWhiteSpace(txtBuscarProducto.Text)
                ? Visibility.Visible
                : Visibility.Collapsed;

            CargarProductos(txtBuscarProducto.Text); // tu función de filtrado
        }



        private void BtnAgregarAlCarrito_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductosVenta.SelectedItem is Producto prod)
            {
                var item = carrito.FirstOrDefault(c => c.Producto.IdProducto == prod.IdProducto);
                if (item == null)
                {
                    carrito.Add(new CarritoItem { Producto = prod, Cantidad = 1 });
                }
                else if (prod.Inventario.StockActual > item.Cantidad)
                {
                    item.Cantidad++;
                }
                dgCarrito.Items.Refresh();
                ActualizarTotal();
            }
        }

        private void ActualizarTotal()
        {
            txtTotalVenta.Text = carrito.Sum(c => c.Subtotal).ToString("C");
        }

        private void BtnRealizarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("Agrega productos al carrito.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var ctx = new MyDbContext())
            {
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = carrito.Sum(c => c.Subtotal),
                    MetodoPago = "Efectivo", // Puedes agregar elección más tarde
                    IdCliente = null, // Esta venta no es a crédito
                    IdVendedor = /* obtén el vendedor actual */ 1
                };
                ctx.Ventas.Add(venta);
                ctx.SaveChanges();

                foreach (var c in carrito)
                {
                    ctx.DetalleVentas.Add(new DetalleVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = c.Producto.IdProducto,
                        Cantidad = c.Cantidad,
                        PrecioUnitario = c.PrecioUnitario,
                        Subtotal = c.Subtotal
                    });

                    // Descuento de stock
                    var inv = ctx.Inventarios.FirstOrDefault(i => i.IdProducto == c.Producto.IdProducto);
                    if (inv != null)
                    {
                        inv.StockActual -= c.Cantidad;
                    }
                }
                ctx.SaveChanges();
            }

            MessageBox.Show("Venta registrada correctamente!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            carrito.Clear();
            dgCarrito.Items.Refresh();
            ActualizarTotal();
            CargarProductos();
        }

    }
}
