using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_LoginForm.Model
{
    [Table("USUARIO")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")] // <-- mapea los nombres reales
        public int IdUsuario { get; set; }

        [Column("id_vendedor")]
        public int? IdVendedor { get; set; }

        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; }

        [Column("contrasena")]
        public string Contrasena { get; set; }

        [Column("rol")]
        public string Rol { get; set; }

        public virtual Vendedor Vendedor { get; set; }
    }
}
