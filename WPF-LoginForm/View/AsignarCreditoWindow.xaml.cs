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

using WPF_LoginForm.Model; // Tu modelo de datos
namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para AsignarCreditoWindow.xaml
    /// </summary>
    public partial class AsignarCreditoWindow : Window
    {
        private readonly Cliente cliente;
        private List<Producto> productosDisponibles;
        private List<Producto> carrito;

        public AsignarCreditoWindow(Cliente clienteSeleccionado)
        {
            InitializeComponent();
            cliente = clienteSeleccionado;
            carrito = new List<Producto>();
            CargarProductos();
        }

        private void CargarProductos()
        {
            using (var db = new MyDbContext())
            {
                productosDisponibles = db.Productos.ToList();
                dgProductos.ItemsSource = productosDisponibles;
            }
        }

        private void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductos.SelectedItem is Producto producto)
            {
                carrito.Add(producto);
                ActualizarCarrito();
            }
        }

        private void BtnQuitarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (dgCarrito.SelectedItem is Producto producto)
            {
                carrito.Remove(producto);
                ActualizarCarrito();
            }
        }

        private void ActualizarCarrito()
        {
            dgCarrito.ItemsSource = null;
            dgCarrito.ItemsSource = carrito;

            decimal total = carrito.Sum(p => p.PrecioVenta);
            txtTotal.Text = $"Bs {total:N2}";
        }

        private void BtnCalcularCuotas_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCantidadCuotas.Text, out int cantidadCuotas) || cantidadCuotas <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida de cuotas.");
                return;
            }

            decimal total = carrito.Sum(p => p.PrecioVenta);
            decimal cuota = total / cantidadCuotas;
            txtMontoCuota.Text = $"Bs {cuota:N2}";
        }

        private void BtnConfirmarCredito_Click(object sender, RoutedEventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("Debe agregar productos al carrito.");
                return;
            }

            if (!int.TryParse(txtCantidadCuotas.Text, out int cuotas) || cuotas <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida de cuotas.");
                return;
            }

            using (var db = new MyDbContext())
            {
                // 1. Crear la venta
                var venta = new Venta
                {
                    IdCliente = cliente.IdCliente,
                    Fecha = DateTime.Now,
                    MetodoPago = "Crédito",
                    Total = carrito.Sum(p => p.PrecioVenta),
                    IdVendedor = 1 // O el que esté logueado
                };
                db.Ventas.Add(venta);
                db.SaveChanges();

                // 2. Agregar detalle de venta
                foreach (var producto in carrito)
                {
                    var detalle = new DetalleVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = producto.IdProducto,
                        Cantidad = 1,
                        PrecioUnitario = producto.PrecioVenta,
                        Subtotal = producto.PrecioVenta
                    };
                    db.DetalleVentas.Add(detalle);
                }

                db.SaveChanges();

                // 3. Crear crédito
                var credito = new Credito
                {
                    IdCliente = cliente.IdCliente,
                    IdVenta = venta.IdVenta,
                    MontoTotal = venta.Total,
                    SaldoPendiente = venta.Total,
                    Cuotas = cuotas,
                    EstadoCredito = "Activo"
                };

                db.Creditos.Add(credito);
                db.SaveChanges();

                MessageBox.Show("Crédito registrado exitosamente.");
                this.Close();
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
