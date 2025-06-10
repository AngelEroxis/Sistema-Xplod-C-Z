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
using System.Data.Entity;
namespace WPF_LoginForm.View
{
    /// <summary>
    /// Lógica de interacción para VentasRealizadasWindow.xaml
    /// </summary>
    public partial class VentasRealizadasWindow : Window
    {
        public VentasRealizadasWindow()
        {
            InitializeComponent();
            CargarVentas();
        }

        private void CargarVentas()
        {
            using (var ctx = new MyDbContext())
            {
                var ventas = ctx.Ventas
                    .Include(v => v.DetalleVentas.Select(dv => dv.Producto))
                    .Include(v => v.Vendedor)
                    .ToList();
                dgVentas.ItemsSource = ventas;
            }

        }

        private void BtnVerDetalles_Click(object sender, RoutedEventArgs e)
        {
            var idVenta = (int)(sender as Button).Tag;
            var detallesWindow = new VentaDetalleWindow(idVenta);
            detallesWindow.ShowDialog();
        }
    }
}
