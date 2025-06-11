using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    [Table("DETALLECOMPRA")]
    public class DetalleCompra
    {
        [Key]
        [Column("id_detalle_compra")]
        public int IdDetalleCompra { get; set; }

        [Column("id_compra")]
        public int IdCompra { get; set; }

        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Column("precio_unitario")]
        public decimal PrecioUnitario { get; set; }

        [Column("subtotal")]
        public decimal SubTotal { get; set; }

        public virtual Compra Compra { get; set; }    // Navegación clara
        public virtual Producto Producto { get; set; }
        //public virtual Inventario Inventario { get; set; }
        //public virtual Proveedor Proveedor { get; set; }
    }
}
