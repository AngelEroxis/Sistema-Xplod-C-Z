using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_LoginForm.Model
{
    [Table("PRODUCTO")]
    public class Producto
    {
        [Key]
        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("descripcion")]
        public string Descripcion { get; set; }

        [Column("precio_venta")]
        public decimal PrecioVenta { get; set; }

        [Column("precio_compra")]
        public decimal PrecioCompra { get; set; }

        [Column("stock_minimo")]
        public int StockMinimo { get; set; }

        [Column("unidad_medida")]
        public string UnidadMedida { get; set; }

        // Propiedad de navegación
        public virtual Inventario Inventario { get; set; }
        public virtual Proveedor Proveedor { get; set; }

    }
}
