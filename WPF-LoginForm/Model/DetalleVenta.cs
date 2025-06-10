using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_LoginForm.Model
{
    [Table("DETALLEVENTA")]
    public class DetalleVenta
    {
        [Key]
        [Column("id_detalle_venta")]
        public int IdDetalleVenta { get; set; }

        [Column("id_venta")]
        public int IdVenta { get; set; }

        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column("subtotal")]
        public decimal Subtotal { get; set; }

        public virtual Venta Venta { get; set; }
        public virtual Producto Producto { get; set; }
    }
}
