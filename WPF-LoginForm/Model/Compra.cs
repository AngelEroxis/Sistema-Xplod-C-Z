using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    [Table("COMPRA")]
    public class Compra
    {
        [Key]
        [Column("id_compra")]
        public int IdCompra { get; set; }

        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }

        [Column("total")]
        public decimal Total { get; set; }

        public virtual Proveedor Proveedor { get; set; }

        public virtual ICollection<DetalleCompra> DetalleCompras { get; set; }

        public Compra()
        {
            DetalleCompras = new HashSet<DetalleCompra>();
        }
    }
}
