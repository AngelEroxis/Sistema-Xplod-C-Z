using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WPF_LoginForm.Model
{
    [Table("PROVEEDOR")]
    public class Proveedor
    {
        [Key]
        [Column("id_proveedor")]
        public int IdProveedor { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("contacto")]
        public string Contacto { get; set; }

        // Propiedad de navegación
        //public virtual Inventario Inventario { get; set; }
        public virtual ICollection<Producto> Productos { get; set; }

    }
}
