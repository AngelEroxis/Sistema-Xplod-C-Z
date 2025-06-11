using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    public class CompraDetalleViewModel
    {
        public int IdProducto { get; set; }
        public string NombreProducto => Producto?.Nombre;
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public void CalcularSubtotal()
        {
            Subtotal = Cantidad * PrecioUnitario;
        }
    }
}
