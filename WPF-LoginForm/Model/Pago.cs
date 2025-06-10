using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_LoginForm.Model
{
    [Table("PAGO")]
    public class Pago
    {
        [Key]
        [Column("id_pago")]
        public int IdPago { get; set; }

        [Column("id_venta")]
        public int IdVenta { get; set; }

        [Column("fecha_pago")]
        public DateTime FechaPago { get; set; }

        [Column("monto_pagado")]
        public decimal MontoPagado { get; set; }

        [Column("metodo_pago")]
        public string MetodoPago { get; set; }
    }
}
