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
using System.Collections.ObjectModel;
using System.Data.Entity;
namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para VentaDetalleWindow.xaml
    /// </summary>
    public partial class VentaDetalleWindow : Window
    {
        public VentaDetalleWindow(int idVenta)
        {
            InitializeComponent();
            CargarDetalle(idVenta);
        }

        private void CargarDetalle(int idVenta)
        {
            Venta venta;

            using (var ctx = new MyDbContext())
            {
                venta = ctx.Ventas
                    .Include(v => v.DetalleVentas.Select(d => d.Producto))
                    .Include(v => v.Vendedor)
                    .FirstOrDefault(v => v.IdVenta == idVenta);
            }

            if (venta != null)
            {
                lblInfoVenta.Text = $"Venta #{venta.IdVenta} - {venta.Fecha:d} - Total: {venta.Total:C}";
                dgDetalleVenta.ItemsSource = venta.DetalleVentas.ToList();
            }
            else
            {
                MessageBox.Show("No se encontró la venta.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
