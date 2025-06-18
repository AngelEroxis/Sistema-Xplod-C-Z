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
    /// Lógica de interacción para ProductoWindow.xaml
    /// </summary>
    public partial class ProductoWindow : Window
    {
        private Producto productoActual;

        public ProductoWindow(Producto producto = null)
        {
            InitializeComponent();
            productoActual = producto;
            CargarProveedores();

            if (productoActual != null)
                CargarDatosProducto();
        }

        private void CargarProveedores()
        {
            using (var ctx = new MyDbContext())
            {
                cbProveedor.ItemsSource = ctx.Proveedores.ToList();
            }
        }

        private void CargarDatosProducto()
        {
            txtNombre.Text = productoActual.Nombre;
            txtDescripcion.Text = productoActual.Descripcion;
            cbProveedor.SelectedValue = productoActual.IdProveedor;
            txtPrecioCompra.Text = productoActual.PrecioCompra.ToString();
            txtPrecioVenta.Text = productoActual.PrecioVenta.ToString();
            txtStockMinimo.Text = productoActual.StockMinimo.ToString();
            txtUnidad.Text = productoActual.UnidadMedida;
            txtStockActual.Text = productoActual.Inventario?.StockActual.ToString() ?? "0";
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || cbProveedor.SelectedItem == null)
            {
                MessageBox.Show("Nombre y proveedor son obligatorios.");
                return;
            }

            try
            {

                using (var ctx = new MyDbContext())
                {
                    // Verificar si el producto ya existe por nombre
                    bool productoExistente = ctx.Productos.Any(prod => prod.Nombre.Equals(txtNombre.Text, StringComparison.OrdinalIgnoreCase));
                    if (productoExistente && productoActual == null)
                    {
                        MessageBox.Show("Ya existe un producto con este nombre.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }


                    Producto p = productoActual ?? new Producto();
                    p.Nombre = txtNombre.Text;
                    p.Descripcion = txtDescripcion.Text;
                    p.IdProveedor = (int)cbProveedor.SelectedValue;
                    p.PrecioCompra = decimal.Parse(txtPrecioCompra.Text);
                    p.PrecioVenta = decimal.Parse(txtPrecioVenta.Text);
                    p.StockMinimo = int.Parse(txtStockMinimo.Text);
                    p.UnidadMedida = txtUnidad.Text;

                    if (productoActual == null)
                    {
                        // Crear nuevo inventario
                        p.Inventario = new Inventario
                        {
                            StockActual = int.Parse(txtStockActual.Text),
                            Ubicacion = "Almacén A1"
                        };
                        ctx.Productos.Add(p);
                    }
                    else
                    {
                        ctx.Entry(p).State = System.Data.Entity.EntityState.Modified;
                        var inv = ctx.Inventarios.FirstOrDefault(i => i.IdProducto == p.IdProducto);
                        if (inv != null)
                            inv.StockActual = int.Parse(txtStockActual.Text);
                    }

                    ctx.SaveChanges();
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                    errorMessage += "\n\n" + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    errorMessage += "\n\n" + ex.InnerException.InnerException.Message;

                MessageBox.Show("Error al guardar:\n" + errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
