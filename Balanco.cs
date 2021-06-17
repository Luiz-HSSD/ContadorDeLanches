using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContadorDeLanches
{
    public class Balanco
    {
        [Key]
        public int Id { get; set; }
        public DateTime Dia { get; set; }
        public int IdPagamento { get; set; }
        public double Total { get; set; }
    }
}
