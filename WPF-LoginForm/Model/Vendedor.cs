using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    [Table("VENDEDOR")]
    public class Vendedor
    {
        [Key]
        [Column("id_vendedor")]
        public int IdVendedor { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("celular")]
        public string Celular { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
