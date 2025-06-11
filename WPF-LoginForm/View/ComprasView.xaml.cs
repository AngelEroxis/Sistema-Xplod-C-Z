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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_LoginForm.Model;
using System.Data.Entity;

using System.Data.Entity.Infrastructure; // ← Este permite ThenInclude en EF6

using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.IO;
using iTextParagraph = iText.Layout.Element.Paragraph;
using iTextTable = iText.Layout.Element.Table;
using iTextTextAlignment = iText.Layout.Properties.TextAlignment;

namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para ComprasView.xaml
    /// </summary>
    public partial class ComprasView : UserControl
    {
        private List<CarritoItem> carrito = new List<CarritoItem>();
        private List<Producto> productos;
        private List<Proveedor> proveedores;

        public ComprasView()
        {
            InitializeComponent();
            CargarProductos();
            CargarProveedores();
            dgCarritoCompra.ItemsSource = carrito;
        }

        private void CargarProductos(string filtro = "")
        {
            using (var ctx = new MyDbContext())
            {
                var query = ctx.Productos.Include(p => p.Inventario).AsQueryable();
                if (!string.IsNullOrWhiteSpace(filtro))
                    query = query.Where(p => p.Nombre.Contains(filtro));
                productos = query.ToList();
                dgProductosCompra.ItemsSource = productos;
            }
        }

        private void CargarProveedores()
        {
            using (var ctx = new MyDbContext())
            {
                proveedores = ctx.Proveedores.ToList();
                cbProveedorCompra.ItemsSource = proveedores;
            }
        }

        private void TxtBuscarProducto_TextChanged(object s, TextChangedEventArgs e)
            => CargarProductos(txtBuscarProducto.Text);

        private void BtnAgregarProveedor_Click(object s, RoutedEventArgs e)
        {
            var win = new ProveedorWindow();
            if (win.ShowDialog() == true) CargarProveedores();
        }

        private void BtnAgregarAlCarrito_Click(object s, RoutedEventArgs e)
        {
            if (dgProductosCompra.SelectedItem is Producto prod)
            {
                var item = carrito.FirstOrDefault(ci => ci.Producto.IdProducto == prod.IdProducto);
                if (item == null) carrito.Add(new CarritoItem { Producto = prod, Cantidad = 1, PrecioUnitario = prod.PrecioCompra });
                else item.Cantidad++;
                dgCarritoCompra.Items.Refresh();
                ActualizarTotalCompra();
            }
        }

        private void ActualizarTotalCompra()
            => txtTotalCompra.Text = $"Bs {carrito.Sum(ci => ci.Subtotal):N2}";

        private void BtnLimpiarCarrito_Click(object s, RoutedEventArgs e)
        {
            carrito.Clear();
            dgCarritoCompra.Items.Refresh();
            ActualizarTotalCompra();
        }

        private void BtnRealizarCompra_Click(object s, RoutedEventArgs e)
        {
            if (!carrito.Any())
            {
                MessageBox.Show("Agrega productos al carrito primero.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!(cbProveedorCompra.SelectedItem is Proveedor prov))
            {
                MessageBox.Show("Selecciona un proveedor.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }


            using (var ctx = new MyDbContext())
            {
                var compra = new Compra
                {
                    Fecha = DateTime.Now,
                    IdProveedor = prov.IdProveedor,
                    Total = carrito.Sum(ci => ci.Subtotal)
                };
                ctx.Compras.Add(compra);
                ctx.SaveChanges();

                foreach (var ci in carrito)
                {
                    ctx.DetalleCompras.Add(new DetalleCompra
                    {
                        IdCompra = compra.IdCompra,
                        IdProducto = ci.Producto.IdProducto,
                        Cantidad = ci.Cantidad,
                        PrecioUnitario = ci.PrecioUnitario,
                        SubTotal = ci.Subtotal
                    });
                    var inv = ctx.Inventarios.FirstOrDefault(i => i.IdProducto == ci.Producto.IdProducto);
                    if (inv != null) inv.StockActual += ci.Cantidad;
                }
                ctx.SaveChanges();
            }

            MessageBox.Show("Compra realizada con éxito!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            BtnLimpiarCarrito_Click(s, null);
            CargarProductos();
        }

        private void BtnVerCompras_Click(object s, RoutedEventArgs e)
        {
            var win = new ComprasRealizadasWindow();
            win.ShowDialog();
        }
    }
}
