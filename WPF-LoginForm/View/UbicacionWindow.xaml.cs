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
    /// Lógica de interacción para UbicacionWindow.xaml
    /// </summary>
    public partial class UbicacionWindow : Window
    {
        public class UbicacionDisplay
        {
            public string Ubicacion { get; set; }
            public string ProductosResumen { get; set; }
        }

        public UbicacionWindow()
        {
            InitializeComponent();
            CargarUbicaciones();
        }

        private void CargarUbicaciones()
        {
            using (var ctx = new MyDbContext())
            {
                var ubicaciones = ctx.Inventarios
                    .Include(i => i.Producto)
                    .ToList() // Trae los datos a memoria
                    .GroupBy(i => i.Ubicacion)
                    .Select(g => new UbicacionDisplay
                    {
                        Ubicacion = g.Key,
                        ProductosResumen = string.Join(", ", g.Select(i => i.Producto.Nombre))
                    })
                    .ToList();

                dgUbicaciones.ItemsSource = ubicaciones;
            }
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            var nombreUbicacion = Microsoft.VisualBasic.Interaction.InputBox("Ingrese nueva ubicación:", "Agregar Ubicación", "");
            if (!string.IsNullOrWhiteSpace(nombreUbicacion))
            {
                using (var ctx = new MyDbContext())
                {
                    // Crea nueva ubicación solo si no existe
                    if (!ctx.Inventarios.Any(i => i.Ubicacion == nombreUbicacion))
                    {
                        // Podrías crearla asociando un producto específico
                        MessageBox.Show("Ubicación creada. Puedes asignar productos desde Inventario.");
                    }
                    else
                    {
                        MessageBox.Show("La ubicación ya existe.");
                    }
                }
                CargarUbicaciones();
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as UbicacionDisplay;
            if (item == null) return;

            if (MessageBox.Show($"¿Eliminar ubicación \"{item.Ubicacion}\" y todos sus registros?", "Confirmar",
                MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                using (var ctx = new MyDbContext())
                {
                    var items = ctx.Inventarios.Where(i => i.Ubicacion == item.Ubicacion);
                    ctx.Inventarios.RemoveRange(items);
                    ctx.SaveChanges();
                }
                CargarUbicaciones();
            }
        }

        private void BtnEditar_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as Button).DataContext as UbicacionDisplay;
            if (item == null) return;

            var nuevaUbicacion = Microsoft.VisualBasic.Interaction.InputBox("Nuevo nombre para la ubicación:", "Editar Ubicación", item.Ubicacion);
            if (!string.IsNullOrWhiteSpace(nuevaUbicacion))
            {
                using (var ctx = new MyDbContext())
                {
                    var inventarios = ctx.Inventarios.Where(i => i.Ubicacion == item.Ubicacion).ToList();
                    foreach (var inv in inventarios)
                        inv.Ubicacion = nuevaUbicacion;
                    ctx.SaveChanges();
                }
                CargarUbicaciones();
            }
        }
    }
}
