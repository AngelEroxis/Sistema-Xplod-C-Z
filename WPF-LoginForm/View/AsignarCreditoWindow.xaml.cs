using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private List<CarritoItem> carrito;
        private List<Inventario> inventarios;


        public AsignarCreditoWindow(Cliente clienteSeleccionado)
        {
            InitializeComponent();
            cliente = clienteSeleccionado;
            carrito = new List<CarritoItem>();
            CargarProductos();
        }

        private void CargarProductos()
        {
            using (var db = new MyDbContext())
            {
                productosDisponibles = db.Productos.Include(p => p.Inventario).ToList();
                inventarios = db.Inventarios.ToList();
                dgProductos.ItemsSource = productosDisponibles;
            }
        }

        private void BtnAgregarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductos.SelectedItem is Producto producto)
            {
                var inventario = inventarios.FirstOrDefault(i => i.IdProducto == producto.IdProducto);
                if (inventario == null)
                {
                    MessageBox.Show("No se encontró stock para este producto.");
                    return;
                }

                var itemExistente = carrito.FirstOrDefault(ci => ci.Producto.IdProducto == producto.IdProducto);

                if (itemExistente != null)
                {
                    if (itemExistente.Cantidad + 1 > inventario.StockActual)
                    {
                        MessageBox.Show("No hay suficiente stock para este producto.");
                        return;
                    }

                    itemExistente.Cantidad++;
                }
                else
                {
                    if (inventario.StockActual < 1)
                    {
                        MessageBox.Show("No hay stock disponible.");
                        return;
                    }

                    carrito.Add(new CarritoItem
                    {
                        Producto = producto,
                        Cantidad = 1,
                        PrecioUnitario = producto.PrecioVenta
                    });

                }

                ActualizarCarrito();
            }
        }


        private void BtnQuitarProducto_Click(object sender, RoutedEventArgs e)
        {
            if (dgCarrito.SelectedItem is CarritoItem item)
            {
                carrito.Remove(item);
                ActualizarCarrito();
            }
        }

        private void ActualizarCarrito()
        {
            dgCarrito.ItemsSource = null;
            dgCarrito.ItemsSource = carrito;
            decimal total = carrito.Sum(ci => ci.Subtotal);
            txtTotal.Text = $"Bs {total:N2}";
        }

        private void BtnAumentar_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is CarritoItem item)
            {
                var inventario = inventarios.FirstOrDefault(i => i.IdProducto == item.Producto.IdProducto);
                if (item.Cantidad + 1 <= inventario.StockActual)
                {
                    item.Cantidad++;
                    ActualizarCarrito();
                }
                else
                {
                    MessageBox.Show("No hay suficiente stock.");
                }
            }
        }

        private void BtnDisminuir_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is CarritoItem item)
            {
                item.Cantidad--;
                if (item.Cantidad <= 0)
                    carrito.Remove(item);

                ActualizarCarrito();
            }
        }



        private void BtnCalcularCuotas_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCantidadCuotas.Text, out int cantidadCuotas) || cantidadCuotas <= 0)
            {
                MessageBox.Show("Ingrese una cantidad válida de cuotas.");
                return;
            }

            decimal total = carrito.Sum(p => p.Subtotal);
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
                    Total = carrito.Sum(p => p.Subtotal),
                    IdVendedor = SesionActual.UsuarioLogueado.IdVendedor ?? 0
                };
                db.Ventas.Add(venta);
                db.SaveChanges();

                // 2. Agregar detalle de venta
                foreach (var item in carrito)
                {
                    var detalle = new DetalleVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = item.Producto.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        Subtotal = item.Subtotal
                    };
                    db.DetalleVentas.Add(detalle);

                    // Descontar del inventario
                    var inventario = db.Inventarios.First(i => i.IdProducto == item.Producto.IdProducto);
                    inventario.StockActual -= item.Cantidad;
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

                foreach (var item in carrito)
                {
                    var detalle = new DetalleVenta
                    {
                        IdVenta = venta.IdVenta,
                        IdProducto = item.Producto.IdProducto,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        Subtotal = item.Subtotal
                    };

                    db.DetalleVentas.Add(detalle);

                    // Descontar del stock
                    var inventario = db.Inventarios.First(i => i.IdProducto == item.Producto.IdProducto);
                    inventario.StockActual -= item.Cantidad;
                }

            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgCarrito_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
