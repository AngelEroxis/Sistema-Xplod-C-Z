using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace WPF_LoginForm.Model
{
    [NotMapped]
    public class DetalleProductoVenta
    {
        public DateTime FechaVenta { get; set; }
        public string NombreProducto { get; set; }
        public string DescripcionProducto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TotalVenta { get; set; }
        public int IdVenta { get; set; }
    }
}
