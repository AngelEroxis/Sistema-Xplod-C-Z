using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Model
{
    [Table("CREDITO")]
    public class Credito
    {
        [Key]
        [Column("id_credito")]
        public int IdCredito { get; set; }

        [Column("id_venta")]
        public int IdVenta { get; set; }

        [Column("id_cliente")]
        public int IdCliente { get; set; }

        [Column("monto_total")]
        public decimal MontoTotal { get; set; }

        [Column("saldo_pendiente")]
        public decimal SaldoPendiente { get; set; }

        [Column("cuotas")]
        public int Cuotas { get; set; }

        [Column("estado_credito")]
        public string EstadoCredito { get; set; }

        public virtual Cliente Cliente { get; set; }


        // Propiedad calculada para la cuota mensual
        [NotMapped]
        public decimal CuotaMensual
        {
            get
            {
                return Cuotas > 0 ? MontoTotal / Cuotas : 0;
            }
        }
    }
}