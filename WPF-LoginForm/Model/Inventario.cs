using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_LoginForm.Model
{
    [Table("INVENTARIO")]
    public class Inventario
    {
        [Key]
        [Column("id_inventario")]
        public int IdInventario { get; set; }

        [Column("id_producto")]
        public int IdProducto { get; set; }

        [Column("stock_actual")]
        public int StockActual { get; set; }

        [Column("ubicacion")]
        public string Ubicacion { get; set; }

        // Propiedad de navegación inversa
        public virtual Producto Producto { get; set; }
    }
}
