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
                // Verificar si el producto ya está en el carrito
                var item = carrito.FirstOrDefault(c => c.Producto.IdProducto == prod.IdProducto);

                // Si el producto no está en el carrito, lo agregamos
                if (item == null)
                {
                    carrito.Add(new CarritoItem
                    {
                        Producto = prod,
                        Cantidad = 1,
                        PrecioUnitario = prod.PrecioVenta // Asignamos el precio de venta al carrito
                    });
                }
                else
                {
                    // Si el producto ya está en el carrito, solo incrementamos la cantidad, pero no debe exceder el stock disponible
                    if (item.Cantidad < prod.Inventario.StockActual)
                    {
                        item.Cantidad++; // Incrementamos la cantidad en 1
                    }
                    else
                    {
                        MessageBox.Show("No hay suficiente stock para agregar más unidades de este producto.", "Stock insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return; // Salimos del método si no hay suficiente stock
                    }
                }

                dgCarrito.Items.Refresh(); // Refrescamos el carrito para mostrar la actualización
                ActualizarTotal(); // Actualizamos el total de la venta
            }
        }



        private void ActualizarTotal()
        {
            txtTotalVenta.Text = $"Bs {carrito.Sum(c => c.Subtotal):N2}";
        }

        private void BtnRealizarVenta_Click(object sender, RoutedEventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("Agrega productos al carrito.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (cbMetodoPago.SelectedItem == null)
            {
                MessageBox.Show("Seleccione un método de pago.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            using (var ctx = new MyDbContext())
            {
                var venta = new Venta
                {
                    Fecha = DateTime.Now,
                    Total = carrito.Sum(c => c.Subtotal),
                    MetodoPago = ((ComboBoxItem)cbMetodoPago.SelectedItem)?.Content?.ToString() ?? "Efectivo",
                    IdCliente = null, // Esta venta no es a crédito
                    IdVendedor = SesionActual.UsuarioLogueado.IdVendedor ?? 0

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
        private void BtnLimpiarCarrito_Click(object sender, RoutedEventArgs e)
        {
            carrito.Clear();
            dgCarrito.Items.Refresh();
            ActualizarTotal();
        }

        private void BtnVerVentas_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new VentasRealizadasWindow();
            ventana.ShowDialog();
        }
    }
}
