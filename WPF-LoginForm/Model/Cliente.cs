using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    [Table("CLIENTE")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("nombre")]
        public string Nombre { get; set; }

        [Column("ci")]
        public string CI { get; set; }

        [Column("num_item")]
        public string NumItem { get; set; }

        [Column("domicilio")]
        public string Domicilio { get; set; }

        [Column("celular")]
        public string Celular { get; set; }

        [Column("empresa_institucion")]
        public string EmpresaInstitucion { get; set; }

        [Column("garante")]
        public string Garante { get; set; }

        [Column("cel_garante")]
        public string CelGarante { get; set; }

        public virtual ICollection<Credito> Creditos { get; set; }

        // NUEVAS PROPIEDADES CALCULADAS
        [NotMapped]
        public decimal MontoTotal => Creditos?.Sum(c => c.MontoTotal) ?? 0;

        [NotMapped]
        public decimal SaldoPendiente => Creditos?.Sum(c => c.SaldoPendiente) ?? 0;

        [NotMapped]
        public int Cuotas => Creditos?.Sum(c => c.Cuotas) ?? 0;

        [NotMapped]
        public string EstadoCredito => Creditos?.FirstOrDefault()?.EstadoCredito ?? "Sin crédito";
    }
}
