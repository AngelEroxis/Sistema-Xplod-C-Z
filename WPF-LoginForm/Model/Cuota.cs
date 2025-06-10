using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPF_LoginForm.Model
{
    // Esta clase no está mapeada a una tabla, es para cálculos
    [NotMapped]
    public class Cuota
    {
        public int NumeroCuota { get; set; }
        public DateTime FechaVencimiento { get; set; }
        public decimal MontoCuota { get; set; }
        public bool EstaPagada { get; set; }
        public DateTime? FechaPago { get; set; }
        public string EstadoCuota { get; set; }
        public int IdCredito { get; set; }
    }
}
