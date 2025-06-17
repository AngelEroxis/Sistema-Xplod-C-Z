using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    public class CarritoItem
    {
        public Producto Producto { get; set; }
        public string Nombre => Producto?.Nombre;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public decimal PrecioVenta => PrecioUnitario;

        //public decimal PrecioUnitario => Producto.PrecioVenta

        public decimal Subtotal => Cantidad * PrecioUnitario;

    }

}
